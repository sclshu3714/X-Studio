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
            Key = "Project",
            Title = "Project",
            SelectedIndex = 0,
            IsGroupEnabled = true,
            DataList = new ObservableCollection<WorkspaceItemModel>()
                        {
                            new WorkspaceItemModel() {
                                Index = 0,
                                Name = "SchoolTimetable",
                                TargetCtlName = "SchoolTimetableCtl",
                                GroupName = "School",
                                IsNew = true,
                                IsVisible = true,
                            },
                             new WorkspaceItemModel() {
                                Index = 1,
                                Name = "CAM3D",
                                GroupName = "CAD",
                                TargetCtlName = "CADCtl",
                                IsNew =true,
                                IsVisible = true,
                            }
                        }
        });
        models.Add(new WorkspaceInfoModel() { 
            Index = 1,
            Key = "Test",
            Title = "Test",
            IsGroupEnabled= true,
        });
        return models;
    }
}
