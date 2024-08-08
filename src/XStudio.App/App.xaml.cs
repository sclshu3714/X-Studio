using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
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

namespace XStudio.App;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : PrismApplication
{
    private IAbpApplicationWithInternalServiceProvider? _abpApplication;

    protected override Window CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.ConfigurationServices();
    }
    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
        //moduleCatalog.AddModule<ModuleNameModule>();
    }


    protected override async void OnStartup(StartupEventArgs e)
    {
        IConfigurationRoot? configuration = ConfigurationInitialized();
        AppSettings.OnInitialized();
        if (configuration != null)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
        else
        {
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
        try
        {
            Log.Information("Starting WPF host.");

            _abpApplication = await AbpApplicationFactory.CreateAsync<AppModule>(options =>
            {
                options.UseAutofac();
                options.Services.AddLogging(loggingBuilder =>
                {
                    loggingBuilder.AddSerilog(dispose: true);
                });
            });

            await _abpApplication.InitializeAsync();

            _abpApplication.Services.GetRequiredService<MainWindow>()?.Show();

        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly!");
        }
    }

    private IConfigurationRoot? ConfigurationInitialized()
    {

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true);
        var configurationRoot = configuration.Build();
        // 检查环境变量是否已设置，如果没有，则设置为开发环境
        var environment = configurationRoot["App:Environment"]; // Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (string.IsNullOrEmpty(environment) ||
            (environment != Environments.Development && environment != Environments.Staging && environment != Environments.Production))
        {
            // 这里可以根据需要设置不同的环境
            environment = "Development";
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);
        }
        else
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);
        }
        if (configuration != null && File.Exists($"appsettings.{environment}.json"))
        {
            configuration.AddJsonFile($"appsettings.{environment}.json", optional: true, true);
        }
        configuration?.AddEnvironmentVariables();
        return configuration?.Build();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        if (_abpApplication != null)
        {
            await _abpApplication.ShutdownAsync();
        }
        Log.CloseAndFlush();
    }

    protected override void OnInitialized()
    {
        //accountService = Container.Resolve<IAccountService>();

        if (SplashScreenInitialized())
        {
            (App.Current.MainWindow.DataContext as INavigationAware)?.OnNavigatedTo(null);
            base.OnInitialized();
        }
    }

    protected override IContainerExtension CreateContainerExtension()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddAutoMapper(config =>
        {
            config.AddProfile<AppMapper>();
            //config.AddProfile<AppCommonMapper>();
        });
        return new DryIocContainerExtension(new Container(CreateContainerRules())
            .WithDependencyInjectionAdapter(serviceCollection));
    }

    protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
    {
        base.ConfigureRegionAdapterMappings(regionAdapterMappings);
        regionAdapterMappings.ConfigurationAdapters(Container);
    }

    private static bool SplashScreenInitialized()
    {
        var dialogService = ContainerLocator.Container.Resolve<IHostDialogService>();
        if (dialogService.ShowWindow(AppViewManager.SplashScreen).Result == ButtonResult.No)
        {
            if (!Authorization())
            {
                Environment.Exit(0);
            }
        }
        return true;
    }

    private static bool Authorization()
    {
        var validationResult = Validation();
        if (validationResult == ButtonResult.Retry)
            return Authorization();

        return validationResult == ButtonResult.OK;

        static ButtonResult Validation()
        {
            var dialogService = ContainerLocator.Container.Resolve<IHostDialogService>();
            return dialogService.ShowWindow(AppViewManager.Login).Result;
        }
    }

    public static void LogOut()
    {
        App.Current.MainWindow.Hide();

        if (SplashScreenInitialized())
        {
            App.Current.MainWindow.Show();
            (App.Current.MainWindow.DataContext as INavigationAware)?.OnNavigatedTo(null);
        }
        else
            Environment.Exit(0);
    }

    public static async Task OnSessionTimeout()
    {
        //await ContainerLocator.Container.Resolve<IAccountService>()
        //    .LogoutAsync();
        await Task.CompletedTask;
    }

    public static async Task OnAccessTokenRefresh(string newAccessToken)
    {
        //await ContainerLocator.Container.Resolve<IAccountStorageService>()
        //    .StoreAccessTokenAsync(newAccessToken);
        await Task.CompletedTask;
    }
}
