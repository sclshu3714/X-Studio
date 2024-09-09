using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XStudio.App.Models;
using XStudio.App.Models.Data;
using XStudio.App.Service;

namespace XStudio.App.ViewModel.Main
{
    public class DisplayAreaInfoViewModel : ViewModelDataBase<DisplayItemModel>
    {
        /// <summary>
        /// 头显示
        /// </summary>
        public string Header { get; set; } = string.Empty;

        /// <summary>
        /// 背景
        /// </summary>
        public string BackgroundToken { get; set; } = string.Empty;

        public DisplayAreaInfoViewModel(DataService dataService) { }

        public RelayCommand<CancelRoutedEventArgs> ClosingCmd => new(Closing);

        private void Closing(CancelRoutedEventArgs? args)
        {
            if (args == null)
            {
                return;
            }
            Growl.Info($"{(args.OriginalSource as TabItem)?.Header} Closing");
        }

        public RelayCommand<RoutedEventArgs> ClosedCmd => new(Closed);

        private void Closed(RoutedEventArgs? args)
        {
            if (args == null)
            {
                return;
            }
            Growl.Info($"{(args.OriginalSource as TabItem)?.Header} Closed");
        }
    }
}
