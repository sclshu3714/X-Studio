using HandyControl.Data;
using HandyControl.Tools;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Documents;
using Volo.Abp.DependencyInjection;
using XStudio.App.Models;
using XStudio.App.Models.Data;
using XStudio.App.ViewModel.Home;
using XStudio.App.ViewModel.Main;
using XStudio.App.Views.UserControls;

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

    public ObservableCollection<WorkspaceInfoViewModel> GetWorkspaceDataList()
    {
        ObservableCollection<WorkspaceInfoViewModel> models = new ObservableCollection<WorkspaceInfoViewModel>();
        models.Add(new WorkspaceInfoViewModel()
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
                                ImageBrush = ResourceHelper.GetResource<object>("Brush.Effects"),
                                IsVisible = true,
                            },
                            new WorkspaceItemModel() {
                                Index = 1,
                                Name = "CAD",
                                GroupName = "Industry",
                                IsNew = true,
                                TargetCtlName = "IndustryCADCtl",
                                ImageBrush = ResourceHelper.GetResource<object>("Brush.Effects"),
                                IsVisible = true,
                            }
                        }
        });
        return models;
    }

    public object? GetWorkspaceUrl(WorkspaceInfoViewModel? workspaceInfoCurrent, WorkspaceItemModel workspaceItemCurrent)
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
    internal WorkspaceInfoViewModel? LoadProjectTemplate(WorkspaceItemModel item)
    {
        switch (item.Name)
        {
            case "SchoolTimetable":
                return new WorkspaceInfoViewModel()
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
                            ImageBrush = ResourceHelper.GetResource<object>("Brush.Effects"),
                            IsVisible = true,
                        },
                        // 节次方案
                        new WorkspaceItemModel() {
                            Index = 1,
                            Name = "Section",
                            GroupName = "Schedule",
                            IsNew = true,
                            TargetCtlName = "SectionCtl",
                            ImageBrush = ResourceHelper.GetResource<object>("Brush.Effects"),
                            IsVisible = true,
                        },
                        // 课程
                        new WorkspaceItemModel() {
                            Index = 1,
                            Name = "Course",
                            GroupName = "Schedule",
                            IsNew = true,
                            TargetCtlName = "CourseCtl",
                            ImageBrush = ResourceHelper.GetResource<object>("Brush.Effects"),
                            IsVisible = true,
                        },
                        // 场所
                        new WorkspaceItemModel() {
                            Index = 1,
                            Name = "Place",
                            GroupName = "Schedule",
                            IsNew = true,
                            TargetCtlName = "PlaceCtl",
                            ImageBrush = ResourceHelper.GetResource<object>("Brush.Effects"),
                            IsVisible = true,
                        },
                        // 课程表
                        new WorkspaceItemModel() {
                            Index = 1,
                            Name = "SchoolTimetable",
                            GroupName = "Schedule",
                            IsNew = true,
                            TargetCtlName = "SchoolTimetableCtl",
                            ImageBrush = ResourceHelper.GetResource<object>("Brush.Effects"),
                            IsVisible = true,
                        }
                    }
                };
            case "CAD":
                return new WorkspaceInfoViewModel()
                {
                    Index = 1,
                    IsGroupEnabled = true,
                    SelectedIndex = -1,
                    Key = "CAD",
                    Title = "CAD"
                };
            default:
                return new WorkspaceInfoViewModel()
                {
                    Index = 1,
                    IsGroupEnabled = true,
                    SelectedIndex = -1,
                    Key = "Test",
                    Title = "Test"
                };
        }
    }

    public ObservableCollection<DisplayAreaInfoViewModel> GeDisplayAreaDataList(HomePageViewModel _homePage)
    {
        return new ObservableCollection<DisplayAreaInfoViewModel>()
        {
            new DisplayAreaInfoViewModel(this){
                Header = "首页",
                Content = new HomePageControl(_homePage)
            },
            new DisplayAreaInfoViewModel(this){
                Header = "显示",
                BackgroundToken = ResourceToken.SuccessBrush
            },
            new DisplayAreaInfoViewModel(this)
            {
                Header = "Success",
                BackgroundToken = ResourceToken.SuccessBrush
            },
            new DisplayAreaInfoViewModel(this)
            {
                Header = "Primary",
                BackgroundToken = ResourceToken.PrimaryBrush
            },
            new DisplayAreaInfoViewModel(this)
            {
                Header = "Warning",
                BackgroundToken = ResourceToken.WarningBrush
            },
            new DisplayAreaInfoViewModel(this)
            {
                Header = "Danger",
                BackgroundToken = ResourceToken.DangerBrush
            },
            new DisplayAreaInfoViewModel(this)
            {
                Header = "Info",
                BackgroundToken = ResourceToken.InfoBrush
            }
        };
    }

    /// <summary>
    /// 获取首页的导航页
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public ObservableCollection<Page> HomePages()
    {
        ObservableCollection<Page> pages = new ObservableCollection<Page>();
        pages.Add(new MainHomePage());
        return pages;
    }

    internal void SetHomePageViewModel(HomePageViewModel homePage)
    {
        throw new NotImplementedException();
    }
}
