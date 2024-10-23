using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XStudio.App.ViewModel;
using XStudio.App.ViewModel.Home;

namespace XStudio.App.Views.UserControls
{
    /// <summary>
    /// HomePageControl.xaml 的交互逻辑
    /// </summary>
    public partial class HomePageControl : UserControl
    {
        private HomePageViewModel _homePage;
        public HomePageControl(HomePageViewModel homePage)
        {
            InitializeComponent();
            AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(Button_Click));
            this._homePage = homePage;
            if (homePage.DataList.Any())
            {
                FrameHome.Navigate(homePage.DataList[0]);
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button button && button.Tag is string tag && _homePage.DataList.Any())
            {
                var index = tag.Value<int>() + 1;
                Page PageCurrent = index >= _homePage.DataList.Count ? _homePage.DataList[0] : _homePage.DataList[index];
                FrameHome.Navigate(PageCurrent);
            }
        }
    }
}
