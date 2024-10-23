using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using HandyControl.Data;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using XStudio.App.Models;
using XStudio.App.Models.Data;
using XStudio.App.Service;

namespace XStudio.App.ViewModel.Main
{
    public class DisplayAreaInfoViewModel : ViewModelDataBase<DisplayItemModel>
    {
        private string _header = string.Empty;
        private string _BackgroundToken = string.Empty;
        private UserControl? _Content = null;
        private DisplayAreaType type = DisplayAreaType.None;

        /// <summary>
        /// 背景
        /// </summary>
        public string BackgroundToken
        {
            get { return _BackgroundToken; }
            set { SetProperty(ref _BackgroundToken, value); }
        }

        /// <summary>
        /// 头显示
        /// </summary>
        public string Header
        {
            get { return _header; }
            set { SetProperty(ref _header, value); }
        }

        /// <summary>
        /// 展示控件
        /// </summary>
        public UserControl? Content
        {
            get { return _Content; }
            set { SetProperty(ref _Content, value); }
        }

        public DisplayAreaType @Type {
            get => type;
            set => SetProperty(ref type, value);
        }

        public DisplayAreaInfoViewModel(DataService dataService) { }

        public RelayCommand<CancelRoutedEventArgs> ClosingCmd => new(Closing);

        public void SetContent(UserControl control) { Content = control; }


        private void Closing(CancelRoutedEventArgs? args)
        {
            if (args == null)
            {
                return;
            }
            Growl.Info($"{(args.OriginalSource as System.Windows.Controls.TabItem)?.Header} Closing");
        }

        public RelayCommand<RoutedEventArgs> ClosedCmd => new(Closed);

        private void Closed(RoutedEventArgs? args)
        {
            if (args == null)
            {
                return;
            }
            Growl.Info($"{(args.OriginalSource as System.Windows.Controls.TabItem)?.Header} Closed");
        }
    }
}
