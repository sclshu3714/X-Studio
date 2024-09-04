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

    public ObservableCollection<DataModel> GetWorkspaceDataList()
    {
        ObservableCollection<DataModel> models = new ObservableCollection<DataModel>();
        models.Add(new DataModel()
        {
            Index = 0,
            Name = "项目",
            IsSelected = true,
            Remark = "工作区支持的项目",
            Type = System.ComponentModel.DataAnnotations.DataType.Custom,
            DataItems = new ObservableCollection<DataModel>()
                        {
                            new DataModel() {
                                Index = 0,
                                Name = "排课",
                                IsSelected = true,
                                Remark = "学校排课项目",
                                Type = System.ComponentModel.DataAnnotations.DataType.Custom
                            }
                        }
        });
        return models;
    }

    internal IEnumerable<WorkspaceInfoModel> GetWorkspaceInfo()
    {
        return new ObservableCollection<WorkspaceInfoModel>();
    }
}
