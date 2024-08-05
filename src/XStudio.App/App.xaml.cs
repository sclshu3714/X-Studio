using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using Serilog;
using Serilog.Events;
using Volo.Abp;
using XStudio.App.Common;
using XStudio.App.Modules;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using XStudio.App.Extensions;
using XStudio.App.Service.Sessions;

namespace XStudio.App;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : PrismApplication
{
    private IAbpApplicationWithInternalServiceProvider? _abpApplication;

    protected override async void OnStartup(StartupEventArgs e)
    {
        AppSettings.OnInitialized();
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

        try
        {
            Log.Information("Starting WPF host.");

            _abpApplication = await AbpApplicationFactory.CreateAsync<AppModule>(options =>
            {
                options.UseAutofac();
                options.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
            });

            await _abpApplication.InitializeAsync();

            _abpApplication.Services.GetRequiredService<MainWindow>()?.Show();

        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly!");
        }
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

    protected override Window CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.ConfigurationServices();
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
