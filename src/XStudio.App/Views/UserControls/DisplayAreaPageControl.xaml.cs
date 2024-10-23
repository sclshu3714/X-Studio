using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class DisplayAreaPageControl : UserControl
    {
        private ViewModelDataBase<Page> _tempPage = new ViewModelDataBase<Page>();
        public DisplayAreaPageControl() {
            InitializeComponent();
            AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(Button_Click));
        }
        public DisplayAreaPageControl(ViewModelDataBase<Page> tempPage)
            : this()
        {
            this._tempPage = tempPage;
            if (_tempPage.DataList.Any())
            {
                FrameHome.Navigate(_tempPage.DataList[0]);
            }
        }

        public void AddPage(Page page) {
            if (_tempPage.DataList == null) { 
                _tempPage.DataList = new ObservableCollection<Page>(); 
            }
            _tempPage.DataList.Add(page);
            FrameHome.Navigate(page);
        }

        public bool ExistPage(string pageName) {
            Page? page = _tempPage.DataList.FirstOrDefault(p => p.Name == pageName);
            return page !=  null && FrameHome.Navigate(page);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button button && button.Tag is string tag && _tempPage.DataList.Any())
            {
                var index = tag.Value<int>() + 1;
                Page PageCurrent = index >= _tempPage.DataList.Count ? _tempPage.DataList[0] : _tempPage.DataList[index];
                FrameHome.Navigate(PageCurrent);
            }
        }
    }
}
