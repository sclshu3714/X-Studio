using Abp.Application.Services.Dto;
using HandyControl.Data;
using HandyControl.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using Volo.Abp.DependencyInjection;
using XStudio.App.Helper;
using XStudio.App.Models;
using XStudio.App.Models.Data;
using XStudio.App.ViewModel.Home;
using XStudio.App.ViewModel.Main;
using XStudio.App.Views.Module;
using XStudio.App.Views.UserControls;

namespace XStudio.App.Service;

public class DataService : ITransientDependency
{
    public ILogger<DataService> Logger { get; set; }
    private readonly ApiHelper apiHelper;
    private string? baseAddress { get; set; }

    public DataService(IConfigurationRoot configuration)
    {
        Logger = NullLogger<DataService>.Instance;
        baseAddress = configuration["AppSettings:BaseUrls"];
        apiHelper = new ApiHelper(baseAddress);
    }

    #region 登录相关
    #endregion

    #region 项目相关

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
                Type = DisplayAreaType.Home,
                Content = new HomePageControl(_homePage)
            },
            new DisplayAreaInfoViewModel(this){
                Header = "显示",
                Type = DisplayAreaType.Display,
                BackgroundToken = ResourceToken.SuccessBrush,
                Content = new DisplayAreaPageControl()
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

    public DisplayAreaInfoViewModel SetHomePageViewModel(HomePageViewModel homePage)
    {
        return new DisplayAreaInfoViewModel(this) {
            Header = "首页",
            Content = new HomePageControl(homePage)
        };
    }

    public ObservableCollection<Page> getTimePeriodPage(ViewModel.Module.TimePeriodViewModel timePeriodViewModel) {
        ObservableCollection<Page> pages = new ObservableCollection<Page>();
        pages.Add(new TimePeriodPage(timePeriodViewModel) {  Name=timePeriodViewModel.Type});
        return pages;
    }

    #endregion

    #region 模块相关
    public async Task<TimePeriod> CreateAsync(TimePeriod input) {
        TimePeriod timePeriod = await apiHelper.PostAsync("api/add", input);
        return timePeriod;
    }

    public async Task<List<TimePeriod>> InsertManyAsync(List<TimePeriod> inputs) {
        List<TimePeriod> timePeriods = await apiHelper.PostAsync("api/adds", inputs);
        return timePeriods;
    }
    public async Task DeleteAsync(Guid id) {
        await apiHelper.DeleteAsync<string>($"api/delete/{id}");
    }

    public async Task DeleteManyAsync(List<Guid> ids) {
        await Task.CompletedTask;
    }

    public async Task<TimePeriod> GetAsync(Guid id) {
        TimePeriod timePeriod = await apiHelper.GetAsync<TimePeriod>($"api/get/{id}");
        return timePeriod;
    }

    public async Task<PagedResultDto<TimePeriod>> GetListAsync(PagedAndSortedResultRequestDto input) {
        PagedResultDto<TimePeriod> timePeriods = await apiHelper.GetListAsync<PagedResultDto<TimePeriod>>($"api/get", input);
        return timePeriods;
    }

    public async Task<TimePeriod> UpdateAsync(Guid id, TimePeriod input) {
        TimePeriod timePeriod = await apiHelper.PutAsync($"api/get", input);
        return timePeriod;
    }
    #endregion
}
