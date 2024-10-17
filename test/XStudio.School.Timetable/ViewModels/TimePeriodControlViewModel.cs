using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using XStudio.School.Timetable.Models;
using XStudio.School.Timetable.Views.DialogViews;

namespace XStudio.School.Timetable.ViewModels {
    public class TimePeriodControlViewModel : MenuItemViewModel {
        private readonly IDialogCoordinator _dialogCoordinator;
        private ObservableCollection<TimePeriod> _timePeriods;
        
        public TimePeriodControlViewModel(IDialogCoordinator dialogCoordinator, HamburgerMenuControlViewModel viewModel) 
            : base(viewModel) {
            _dialogCoordinator = dialogCoordinator;
            TimePeriods = new ObservableCollection<TimePeriod>();
            AddCommand = new DelegateCommand(AddTimePeriod);
            UpCommand = new DelegateCommand<TimePeriod>(MoveUp);
            DownCommand = new DelegateCommand<TimePeriod>(MoveDown);
            DeleteCommand = new DelegateCommand<TimePeriod>(RemoveTimePeriod);
        }

        public ObservableCollection<TimePeriod> TimePeriods {
            get { return _timePeriods; }
            set { SetProperty(ref _timePeriods, value); }
        }

        #region Commands

        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand<TimePeriod> UpCommand { get; private set; }
        public DelegateCommand<TimePeriod> DownCommand { get; private set; }
        public DelegateCommand<TimePeriod> DeleteCommand { get; private set; }

        private async void AddTimePeriod() {
            var dialog = new TimePeriodWindow(_dialogCoordinator);
            dialog.Owner = System.Windows.Application.Current.MainWindow;
            dialog.SetOrder(TimePeriods.Any() ? TimePeriods.Max(x => x.Order) + 1 : 0);
            if (dialog.ShowDialog() == true) {
                TimePeriods.Add(dialog.TimePeriodModel.TimePeriod);
            }
            await Task.CompletedTask;
        }

        private void MoveUp(TimePeriod timePeriod) {
            int index = TimePeriods.IndexOf(timePeriod);
            if (index > 0) {
                // 交换当前项和上一个项的 Order 和 Code
                var previousItem = TimePeriods[index - 1];

                // 交换 Order
                int tempOrder = timePeriod.Order;
                timePeriod.Order = previousItem.Order;
                previousItem.Order = tempOrder;

                // 交换 Code
                string tempCode = timePeriod.Code;
                timePeriod.Code = previousItem.Code;
                previousItem.Code = tempCode;

                // 移动项
                TimePeriods.Move(index, index - 1);
            }
        }

        private void MoveDown(TimePeriod timePeriod) {
            int index = TimePeriods.IndexOf(timePeriod);
            if (index < TimePeriods.Count - 1) {
                // 交换当前项和下一个项的 Order 和 Code
                var nextItem = TimePeriods[index + 1];

                // 交换 Order
                int tempOrder = timePeriod.Order;
                timePeriod.Order = nextItem.Order;
                nextItem.Order = tempOrder;

                // 交换 Code
                string tempCode = timePeriod.Code;
                timePeriod.Code = nextItem.Code;
                nextItem.Code = tempCode;

                // 移动项
                TimePeriods.Move(index, index + 1);
            }
        }

        private async void RemoveTimePeriod(TimePeriod period) {
            if (period != null) {
                TimePeriods.Remove(period);
            }
            await Task.CompletedTask;
        }

        #endregion
    }
}
