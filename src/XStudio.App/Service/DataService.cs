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
        models.Add(new WorkspaceInfoModel() { 
             Index= 1, IsGroupEnabled = true, SelectedIndex = -1, Key = "Test", Title = "Test"
        });
        return models;
    }

    public object? GetWorkspaceUrl(WorkspaceInfoModel? workspaceInfoCurrent, WorkspaceItemModel workspaceItemCurrent)
    {
        //throw new NotImplementedException();
        return null;
    }
}
