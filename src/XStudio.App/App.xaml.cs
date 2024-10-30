using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Prism.DryIoc;
using Prism.Ioc;
using Serilog;
using Serilog.Events;
using Serilog.Settings.Configuration;
using Volo.Abp;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using XStudio.App.Common;
using XStudio.App.Modules;
using XStudio.App.Extensions;
using XStudio.App.Service.Sessions;
using Prism.Modularity;
using HandyControl.Data;
using HandyControl.Tools;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Runtime;
using System.Threading;
using XStudio.App.Models.Data;
using XStudio.App.Helper;
using Prism.Dialogs;
using Prism.Navigation.Regions;
using Prism.Container.DryIoc;
using System.Windows.Threading;

namespace XStudio.App;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : PrismApplication {
    private IAbpApplicationWithInternalServiceProvider? _abpApplication;

    protected override Window CreateShell() {
        return Container.Resolve<MainWindow>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry) {
        containerRegistry.ConfigurationServices();
    }

    protected override async void OnStartup(StartupEventArgs e) {
        IConfigurationRoot? configuration = ConfigurationInitialized();
        AppSettings.Instance.OnInitialized();
        ApplyConfiguration();
        if (configuration != null) {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
        else {
            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Debug()
#else
                .MinimumLevel.Information()
#endif
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Async(c => c.File("Logs/logs.txt"))
                .CreateLogger();
        }
        try {
            Log.Information("Starting WPF host.");
            // 在应用程序启动时设置全局异常处理
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            _abpApplication = await AbpApplicationFactory.CreateAsync<AppModule>(options => {
                options.UseAutofac();
                options.Services.AddLogging(loggingBuilder => {
                    loggingBuilder.AddSerilog(dispose: true);
                });
            });

            await _abpApplication.InitializeAsync();
            // 注册配置到服务容器
            if (configuration != null) {
                configuration.Bind(AppSettings.Instance);
            }
            _abpApplication.Services.GetRequiredService<MainWindow>()?.Show();

        }
        catch (Exception ex) {
            Log.Fatal(ex, "Host terminated unexpectedly!");
        }
    }

    private IConfigurationRoot? ConfigurationInitialized() {

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true);
        var configurationRoot = configuration.Build();
        // 检查环境变量是否已设置，如果没有，则设置为开发环境
        var environment = configurationRoot["App:Environment"]; // Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (string.IsNullOrEmpty(environment) ||
            (environment != Environments.Development && environment != Environments.Staging && environment != Environments.Production)) {
            // 这里可以根据需要设置不同的环境
            environment = "Development";
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);
        }
        else {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);
        }
        if (configuration != null && File.Exists($"appsettings.{environment}.json")) {
            configuration.AddJsonFile($"appsettings.{environment}.json", optional: true, true);
        }
        configuration?.AddEnvironmentVariables();
        return configuration?.Build();
    }

    protected override async void OnExit(ExitEventArgs e) {
        if (_abpApplication != null) {
            await _abpApplication.ShutdownAsync();
        }
        Log.CloseAndFlush();
    }

    protected override void OnInitialized() {
        //accountService = Container.Resolve<IAccountService>();

        if (SplashScreenInitialized()) {
            (App.Current.MainWindow.DataContext as INavigationAware)?.OnNavigatedTo(null);
            base.OnInitialized();
        }
    }

    protected override IContainerExtension CreateContainerExtension() {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddAutoMapper(config => {
            config.AddProfile<AppMapper>();
            //config.AddProfile<AppCommonMapper>();
        });
        return new DryIocContainerExtension(new Container(CreateContainerRules())
            .WithDependencyInjectionAdapter(serviceCollection));
    }

    protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings) {
        base.ConfigureRegionAdapterMappings(regionAdapterMappings);
        //regionAdapterMappings..ConfigurationAdapters(Container);
    }

    private static bool SplashScreenInitialized() {
        var dialogService = ContainerLocator.Container.Resolve<IHostDialogService>();
        if (dialogService.ShowWindow(AppViewManager.SplashScreen).Result == ButtonResult.No) {
            if (!Authorization()) {
                Environment.Exit(0);
            }
        }
        return true;
    }

    private static bool Authorization() {
        var validationResult = Validation();
        if (validationResult == ButtonResult.Retry)
            return Authorization();

        return validationResult == ButtonResult.OK;

        static ButtonResult Validation() {
            var dialogService = ContainerLocator.Container.Resolve<IHostDialogService>();
            return dialogService.ShowWindow(AppViewManager.Login).Result;
        }
    }

    public static void LogOut() {
        App.Current.MainWindow.Hide();

        if (SplashScreenInitialized()) {
            App.Current.MainWindow.Show();
            (App.Current.MainWindow.DataContext as INavigationAware)?.OnNavigatedTo(null);
        }
        else
            Environment.Exit(0);
    }

    public static async Task OnSessionTimeout() {
        //await ContainerLocator.Container.Resolve<IAccountService>()
        //    .LogoutAsync();
        await Task.CompletedTask;
    }

    public static async Task OnAccessTokenRefresh(string newAccessToken) {
        //await ContainerLocator.Container.Resolve<IAccountStorageService>()
        //    .StoreAccessTokenAsync(newAccessToken);
        await Task.CompletedTask;
    }


    #region demo
    private static Mutex? AppMutex = null;
    internal void UpdateSkin(SkinType skin) {
        var skins0 = Resources.MergedDictionaries[0];
        skins0.MergedDictionaries.Clear();
        skins0.MergedDictionaries.Add(XStudioResourceHelper.GetSkin(skin));
        skins0.MergedDictionaries.Add(XStudioResourceHelper.GetSkin(typeof(App).Assembly, "Resources/Themes", skin));

        var skins1 = Resources.MergedDictionaries[1];
        skins1.MergedDictionaries.Clear();
        skins1.MergedDictionaries.Add(new ResourceDictionary {
            Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml")
        });
        skins1.MergedDictionaries.Add(new ResourceDictionary {
            Source = new Uri("pack://application:,,,/XStudio.App;component/Resources/Themes/Theme.xaml")
        });

        Current.MainWindow?.OnApplyTemplate();
    }

    private void ApplyConfiguration() {
        ShutdownMode = ShutdownMode.OnMainWindowClose;
        GlobalData.Init();
        if (GlobalData.Config == null) { return; }
        ConfigHelper.Instance.SetLang(GlobalData.Config.Lang);
        //XStudio.App.Properties.Langs.LangProvider.Culture = new CultureInfo(GlobalData.Config.Lang);

        if (GlobalData.Config.Skin != SkinType.Default) {
            UpdateSkin(GlobalData.Config.Skin);
        }

        ConfigHelper.Instance.SetWindowDefaultStyle();
        ConfigHelper.Instance.SetNavigationWindowDefaultStyle();

#if NET40
        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
#else
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
#endif
    }

    private static void OpenSplashScreen() {
        var splashScreen = new SplashScreen("Resources/Img/Cover.png");
        splashScreen.Show(true);
    }

    private static void EnsureProfileOptimization() {
#if !NET40
        var cachePath = $"{AppDomain.CurrentDomain.BaseDirectory}Cache";
        if (!Directory.Exists(cachePath)) {
            Directory.CreateDirectory(cachePath);
        }
        ProfileOptimization.SetProfileRoot(cachePath);
        ProfileOptimization.StartProfile("Profile");
#endif
    }

    private void EnsureSingleton() {
        AppMutex = new Mutex(true, "XStudio.App", out var createdNew);

        if (createdNew) {
            return;
        }

        var current = Process.GetCurrentProcess();

        foreach (var process in Process.GetProcessesByName(current.ProcessName)) {
            if (process.Id != current.Id) {
                Win32Helper.SetForegroundWindow(process.MainWindowHandle);
                break;
            }
        }

        Shutdown();
    }

    private static void UpdateRegistry() {
        var processModule = Process.GetCurrentProcess().MainModule;
        if (processModule != null) {
            var registryFilePath = $"{Path.GetDirectoryName(processModule.FileName)}\\Registry.reg";
            if (!File.Exists(registryFilePath)) {
                var streamResourceInfo = GetResourceStream(new Uri("pack://application:,,,/Resources/Registry.txt"));
                if (streamResourceInfo != null) {
                    using var reader = new StreamReader(streamResourceInfo.Stream);
                    var registryStr = reader.ReadToEnd();
                    var newRegistryStr = registryStr.Replace("#", processModule.FileName.Replace("\\", "\\\\"));
                    File.WriteAllText(registryFilePath, newRegistryStr);
                    Process.Start(new ProcessStartInfo("cmd", $"/c {registryFilePath}") {
                        UseShellExecute = false,
                        CreateNoWindow = true
                    });
                }
            }
        }
    }

    private static void UpdateApp() {
        const string api = "https://github.com/handyorg/handycontrol/releases/latest";

        try {
            var mainDirectory = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\'));
            if (mainDirectory != null && Path.Combine(mainDirectory, "Update.exe") is string updateExePath &&
                File.Exists(updateExePath)) {
                Task.Factory.StartNew(() => Process.Start(updateExePath, $"--update={api}"));
            }
        }
        catch {
            // ignored
        }
    }
    #endregion

    #region 异常捕获
    private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) {
        // 处理UI线程中抛出的未捕获异常
        HandleException(e.Exception);
        e.Handled = true; // 表示异常已处理
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
        // 处理非UI线程中抛出的未捕获异常
        Exception? exception = e.ExceptionObject as Exception;
        HandleException(exception);
    }

    private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e) {
        // 处理使用Task生成的异常
        HandleException(e.Exception);
        e.SetObserved(); // 通知系统异常已处理
    }

    private void HandleException(Exception? ex) {
        // 在这里实现异常处理逻辑，例如显示错误对话框、记录日志等
        // MessageBox.Show($"发生异常: {ex.Message}\n\n异常类型: {ex.GetType().Name}");
        HandyControl.Controls.Growl.Clear(MessageToken.GrowlMainWindow);
        if (ex == null) {
            HandyControl.Controls.Growl.Error("发生了未知错误", MessageToken.GrowlMainWindow);
            return;
        }
        HandyControl.Controls.Growl.Error($"{ex.Message}", MessageToken.GrowlMainWindow);
        Log.Error(ex, ex.Message);
    }
    #endregion
}
