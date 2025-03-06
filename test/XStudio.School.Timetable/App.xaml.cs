using MahApps.Metro.Controls.Dialogs;
using Prism.Ioc;
using Serilog;
using System.Windows;
using XStudio.School.Timetable.ViewModels;
using XStudio.School.Timetable.Views;
using XStudio.School.Timetable.Views.ContentViews;

namespace XStudio.School.Timetable {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App {
        public App() {
            // 配置Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", 
                   rollingInterval: RollingInterval.Day,
                   rollOnFileSizeLimit: true,
                   fileSizeLimitBytes: 10 * 1024 * 1024, // 10MB
                   retainedFileCountLimit: 7)
                .CreateLogger();
        }
        protected override Window CreateShell() {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry) {
            containerRegistry.RegisterForNavigation<HamburgerMenuControl>();
            containerRegistry.RegisterForNavigation<TimePeriodControl>();
            containerRegistry.RegisterForNavigation<TimetableControl>();
            // 注册对话框协调器
            containerRegistry.RegisterInstance<IDialogCoordinator>(DialogCoordinator.Instance);
        }

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            // 使用Serilog记录应用程序启动信息
            Log.Information("应用程序已启动");
        }

        protected override void OnExit(ExitEventArgs e) {
            base.OnExit(e);
            // 使用Serilog记录应用程序退出信息
            Log.Information("应用程序已退出");
            // 清理Serilog
            Log.CloseAndFlush();
        }
    }
}
