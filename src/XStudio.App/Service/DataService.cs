using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using Volo.Abp.DependencyInjection;
using XStudio.App.Models;
using XStudio.App.ViewModel.Main;

namespace XStudio.App.Service;

public class DataService : ITransientDependency
{
    public ILogger<DataService> Logger { get; set; }

    public DataService()
    {
        Logger = NullLogger<DataService>.Instance;
    }
    public string SayHello()
    {
        Logger.LogInformation("Call SayHello");
        return "Hello world!";
    }

    public ObservableCollection<WorkspaceInfoModel> GetWorkspaceDataList()
    {
        ObservableCollection<WorkspaceInfoModel> models = new ObservableCollection<WorkspaceInfoModel>();
        models.Add(new WorkspaceInfoModel()
        {
            Index = 0,
            Title = "Project",
            Key = "Project",
            SelectedIndex = 0,
            IsGroupEnabled = true,
            DataList = new ObservableCollection<WorkspaceItemModel>()
                        {
                            new WorkspaceItemModel() {
                                Index = 0,
                                Name = "SchoolTimetable",
                                GroupName = "Education",
                                IsNew = true,
                                TargetCtlName = "SchoolTimetableCtl",
                                IsVisible = true,
                            },
                            new WorkspaceItemModel() {
                                Index = 1,
                                Name = "CAD",
                                GroupName = "Industry",
                                IsNew = true,
                                TargetCtlName = "IndustryCADCtl",
                                IsVisible = true,
                            }
                        }
        });
        return models;
    }

    public object? GetWorkspaceUrl(WorkspaceInfoModel? workspaceInfoCurrent, WorkspaceItemModel workspaceItemCurrent)
    {
        //throw new NotImplementedException();
        return null;
    }

    /// <summary>
    /// 加载模板
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    internal WorkspaceInfoModel? LoadProjectTemplate(WorkspaceItemModel item)
    {
        switch (item.Name)
        {
            case "SchoolTimetable":
                return new WorkspaceInfoModel()
                {
                    Index = 1,
                    IsGroupEnabled = true,
                    SelectedIndex = -1,
                    Key = "SchoolTimetable",
                    Title = "SchoolTimetable",
                    DataList = new ObservableCollection<WorkspaceItemModel>() {
                        // 时段
                        new WorkspaceItemModel() {
                            Index = 0,
                            Name = "TimePeriod",
                            GroupName = "Schedule",
                            IsNew = true,
                            TargetCtlName = "TimePeriodCtl",
                            IsVisible = true,
                        },
                        // 节次方案
                        new WorkspaceItemModel() {
                            Index = 1,
                            Name = "Section",
                            GroupName = "Schedule",
                            IsNew = true,
                            TargetCtlName = "SectionCtl",
                            IsVisible = true,
                        },
                        // 课程
                        new WorkspaceItemModel() {
                            Index = 1,
                            Name = "Course",
                            GroupName = "Schedule",
                            IsNew = true,
                            TargetCtlName = "CourseCtl",
                            IsVisible = true,
                        },
                        // 场所
                        new WorkspaceItemModel() {
                            Index = 1,
                            Name = "Place",
                            GroupName = "Schedule",
                            IsNew = true,
                            TargetCtlName = "PlaceCtl",
                            IsVisible = true,
                        },
                        // 课程表
                        new WorkspaceItemModel() {
                            Index = 1,
                            Name = "SchoolTimetable",
                            GroupName = "Schedule",
                            IsNew = true,
                            TargetCtlName = "SchoolTimetableCtl",
                            IsVisible = true,
                        }
                    }
                };
            case "CAD":
                return new WorkspaceInfoModel()
                {
                    Index = 1,
                    IsGroupEnabled = true,
                    SelectedIndex = -1,
                    Key = "CAD",
                    Title = "CAD"
                };
            default:
                return new WorkspaceInfoModel()
                {
                    Index = 1,
                    IsGroupEnabled = true,
                    SelectedIndex = -1,
                    Key = "Test",
                    Title = "Test"
                };
        }
    }
}
