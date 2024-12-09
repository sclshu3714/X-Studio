using AutoMapper;
using HandyControl.Data;
using HandyControl.Tools;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;
using XStudio.App.Common;
using XStudio.App.Helper;
using XStudio.App.Models.Data;
using XStudio.App.Models.Users;
using XStudio.App.ViewModel.Home;
using XStudio.App.ViewModel.Main;
using XStudio.App.ViewModel.Module;
using XStudio.App.ViewModel.Module.Schools;
using XStudio.App.ViewModel.Users;
using XStudio.App.Views.Module;
using XStudio.App.Views.Module.Schools;
using XStudio.App.Views.UserControls;
using XStudio.Users;

namespace XStudio.App.Service;

public class DataService : ITransientDependency
{
    public ILogger<DataService> Logger { get; set; }
    private readonly ApiHelper apiHelper;
    private string? RootUrl { get; set; }
    //private readonly IMapper ObjectMapper;

    public DataService()
    {
        //ObjectMapper = mapper;
        Logger = NullLogger<DataService>.Instance;
        RootUrl = AppSettings.Instance.RootUrl;
        apiHelper = new ApiHelper(RootUrl);
    }

    #region 登录相关
    public async Task<UserViewModel?> LoginAsync(string userName, string password, bool rememberMe)
    {
        var input = new LoginInfo {
            UserNameOrEmailAddress = userName,
            Password = password,
            RememberMe = rememberMe
        };
        var request = new TokenRequest { 
            ClientId = "XStudio_App",
            //ClientSecret = "XStudio",
            Scope = "XStudio",
            UserName = userName,
            Password = password,
            GrantType = "password",
        };
        UserViewModel? user = await apiHelper.LoginAsync<UserViewModel>("api/xstudio/v1/login", input);
        TokenResponse? tokenResponse = null;
        if (user != null && user.ExtraProperties.ContainsKey("AccessToken")) {
            tokenResponse = await apiHelper.TokenAsync("connect/token", request);
        }
        else if(user != null &&
                user.ExtraProperties.ContainsKey("accessToken") &&
                user.ExtraProperties.ContainsKey("refreshToken") && 
                user.ExtraProperties.ContainsKey("tokenType") && 
                user.ExtraProperties.ContainsKey("expiresIn")) {
            tokenResponse = new TokenResponse();
            tokenResponse.AccessToken = user.ExtraProperties["accessToken"]?.ToString() ?? string.Empty;
            tokenResponse.RefreshToken = user.ExtraProperties["refreshToken"]?.ToString() ?? string.Empty;
            tokenResponse.TokenType = user.ExtraProperties["tokenType"]?.ToString() ?? string.Empty;
            tokenResponse.ExpiresIn = (long?)user.ExtraProperties["expiresIn"] ?? 0;
        }
        if (user != null && tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.AccessToken)) {
            user.TokenResponse.SetTokenResponse(tokenResponse);
            apiHelper.SetAuthorizationHeader(tokenResponse.AccessToken);
        }
        return user;
    }

    public async Task LogoutAsync()
    {
        await apiHelper.GetAsync<object>("api/account/logout");
        apiHelper.SetAuthorizationHeader(null);
    }
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
                Visibility = Visibility.Visible,
                Type = DisplayAreaType.Home,
                Content = new HomePageControl(_homePage)
            },
            new DisplayAreaInfoViewModel(this){
                Header = "显示",
                Visibility= Visibility.Collapsed,
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

    #endregion

    #region 模块-时段
    public ObservableCollection<Page> getTimePeriodPage(TimePeriodPageViewModel timePeriodViewModel) {
        ObservableCollection<Page> pages = new ObservableCollection<Page>();
        pages.Add(new TimePeriodPage(timePeriodViewModel) { Name = timePeriodViewModel.Type });
        return pages;
    }

    public async Task<TimePeriodViewModel?> CreateAsync(TimePeriodViewModel input) {
        TimePeriodViewModel? timePeriod = await apiHelper.PostAsync("api/xstudio/v1/TimePeriod/add", input);
        return timePeriod;
    }

    public async Task<List<TimePeriodViewModel>?> InsertManyAsync(List<TimePeriodViewModel> inputs) {
        List<TimePeriodViewModel>? timePeriods = await apiHelper.PostManyAsync("api/xstudio/v1/TimePeriod/adds", inputs);
        return timePeriods;
    }
    public async Task DeleteAsync(Guid id) {
        await apiHelper.DeleteAsync<string>($"api/xstudio/v1/School/delete/{id}");
    }

    public async Task DeleteManyAsync(List<Guid> ids) {
        await apiHelper.DeleteManyAsync<string>($"api/xstudio/v1/School/deletes", ids);
        await Task.CompletedTask;
    }

    public async Task<TimePeriodViewModel?> GetAsync(Guid id) {
        TimePeriodViewModel? timePeriod = await apiHelper.GetAsync<TimePeriodViewModel>($"api/xstudio/v1/TimePeriod/{id}");
        return timePeriod;
    }

    public async Task<PagedResultDto<TimePeriodViewModel>?> GetListAsync(PagedAndSortedResultRequestDto input) {
        PagedResultDto<TimePeriodViewModel>? timePeriods = await apiHelper.GetListAsync<PagedResultDto<TimePeriodViewModel>>($"/api/xstudio/v1/TimePeriod/list", input);
        return timePeriods;
    }

    public async Task<TimePeriodViewModel?> UpdateAsync(Guid id, TimePeriodViewModel input) {
        TimePeriodViewModel?  timePeriod = await apiHelper.PutAsync($"api/xstudio/v1/TimePeriod/update", input);
        return timePeriod;
    }

    #endregion


    #region 模块-节次方案
    #region 模块-节次


    public ObservableCollection<Page> getSchedulePage(ScheduleSectionPageViewModel sectionPageViewModel) {
        ObservableCollection<Page> pages = new ObservableCollection<Page>();
        pages.Add(new ScheduleSectionPage(sectionPageViewModel) { Name = sectionPageViewModel.Type });
        return pages;
    }


    public async Task<ScheduleViewModel?> CreateScheduleAsync(ScheduleViewModel input) {
        return await apiHelper.PostAsync("api/xstudio/v1/Schedule/add", input);
    }

    public async Task<List<ScheduleViewModel>?> InsertManyScheduleAsync(List<ScheduleViewModel> inputs) {
        return await apiHelper.PostManyAsync("api/xstudio/v1/Schedule/adds", inputs);
    }
    public async Task DeleteScheduleAsync(Guid id) {
        await apiHelper.DeleteAsync<string>($"api/xstudio/v1/Schedule/delete/{id}");
    }

    public async Task DeleteManyScheduleAsync(List<Guid> ids) {
        await apiHelper.DeleteManyAsync<string>($"api/xstudio/v1/Schedule/deletes", ids);
        await Task.CompletedTask;
    }

    public async Task<ScheduleViewModel?> GetScheduleAsync(Guid id) {
        return await apiHelper.GetAsync<ScheduleViewModel>($"api/xstudio/v1/Schedule/{id}"); ;
    }

    public async Task<PagedResultDto<ScheduleViewModel>?> GetScheduleListAsync(PagedAndSortedResultRequestDto input) {
        return await apiHelper.GetListAsync<PagedResultDto<ScheduleViewModel>>($"/api/xstudio/v1/Schedule/list", input);
    }

    public async Task<ScheduleViewModel?> UpdateScheduleAsync(Guid id, ScheduleViewModel input) {
        return await apiHelper.PutAsync($"api/xstudio/v1/Schedule/update", input);
    }

    #endregion
    #endregion

    #region 模块-节次
    public async Task<SectionViewModel?> CreateSectionAsync(SectionViewModel input) {
        return await apiHelper.PostAsync("api/xstudio/v1/Section/add", input);
    }

    public async Task<List<SectionViewModel>?> InsertManySectionAsync(List<SectionViewModel> inputs) {
        return await apiHelper.PostManyAsync("api/xstudio/v1/Section/adds", inputs);
    }
    public async Task DeleteSectionAsync(Guid id) {
        await apiHelper.DeleteAsync<string>($"api/xstudio/v1/Section/delete/{id}");
    }

    public async Task DeleteManySectionAsync(List<Guid> ids) {
        await apiHelper.DeleteManyAsync<string>($"api/xstudio/v1/Section/deletes", ids);
        await Task.CompletedTask;
    }

    public async Task<SectionViewModel?> GetSectionAsync(Guid id) {
        return await apiHelper.GetAsync<SectionViewModel>($"api/xstudio/v1/Section/{id}"); ;
    }

    public async Task<PagedResultDto<SectionViewModel>?> GetSectionListAsync(string scheduleCode, PagedAndSortedResultRequestDto input) {
        return await apiHelper.GetListAsync<PagedResultDto<SectionViewModel>>($"/api/xstudio/v1/Section/list", input);
    }

    public async Task<SectionViewModel?> UpdateSectionAsync(Guid id, SectionViewModel input) {
        return await apiHelper.PutAsync($"api/xstudio/v1/Section/update", input);
    }

    
    #endregion
}
