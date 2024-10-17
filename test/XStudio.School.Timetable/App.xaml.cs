using MahApps.Metro.Controls.Dialogs;
using Prism.Ioc;
using System.Windows;
using XStudio.School.Timetable.ViewModels;
using XStudio.School.Timetable.Views;
using XStudio.School.Timetable.Views.ContentViews;

namespace XStudio.School.Timetable {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App {
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
    }
}
