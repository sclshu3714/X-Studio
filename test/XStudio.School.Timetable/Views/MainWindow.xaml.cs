using MahApps.Metro.Controls;
using Prism.Ioc;
using Prism.Regions;
using System.Windows;

namespace XStudio.School.Timetable.Views {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow {
        private IContainerExtension _container;
        private IRegionManager _regionManager;
        public MainWindow(IContainerExtension container, IRegionManager regionManager) {
            InitializeComponent();
            _container = container;
            _regionManager = regionManager;

            this.SourceInitialized += (s, e) => {
                //view discovery
                regionManager.RegisterViewWithRegion("ContentRegion", typeof(HamburgerMenuControl));
            };
        }
    }
}
