using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using XStudio.App.Models.Data;
using XStudio.App.Service;
using XStudio.App.Views.Module;

namespace XStudio.App.ViewModel.Module {
    public class TimePeriodViewModel : ViewModelDataBase<Page> {
        private readonly DataService _dataService;
        private string _type;
        private ObservableCollection<TimePeriod> _timePeriods;
        

        public TimePeriodViewModel(DataService dataService,string type) {
            _dataService = dataService;
            _type = type;
            DataList = dataService.getTimePeriodPage(this);
            _timePeriods = new ObservableCollection<TimePeriod>();
            SaveCommand = new DelegateCommand<TimePeriodViewModel>(SaveTimePeriod);
            AddCommand = new DelegateCommand(AddTimePeriod);
            UpCommand = new DelegateCommand<TimePeriod>(MoveUp);
            DownCommand = new DelegateCommand<TimePeriod>(MoveDown);
            DeleteCommand = new DelegateCommand<TimePeriod>(RemoveTimePeriod);
            LoadData();
        }

        private async void LoadData() {
            var data = await _dataService.GetListAsync(new Abp.Application.Services.Dto.PagedAndSortedResultRequestDto() { MaxResultCount = 100, SkipCount = 0, Sorting = "Order" });
            if (data != null && data.Items.Any()) {
                TimePeriods.AddRange(data.Items);
            }
        }

        public string @Type {
            get => _type;
            set => SetProperty(ref _type, value);
        }
        public ObservableCollection<TimePeriod> TimePeriods {
            get { return _timePeriods; }
            set { SetProperty(ref _timePeriods, value); }
        }

        #region Commands
        public DelegateCommand<TimePeriodViewModel> SaveCommand { get; private set; }
        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand<TimePeriod> UpCommand { get; private set; }
        public DelegateCommand<TimePeriod> DownCommand { get; private set; }
        public DelegateCommand<TimePeriod> DeleteCommand { get; private set; }

        private async void AddTimePeriod() {
            var dialog = new TimePeriodWindow();
            dialog.Owner = System.Windows.Application.Current.MainWindow;
            dialog.SetOrder(TimePeriods.Any() ? TimePeriods.Max(x => x.Order) + 1 : 0);
            if (dialog.ShowDialog() == true) {
                TimePeriods.Add(dialog.TimePeriodModel);
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
                int index = TimePeriods.IndexOf(period);
                for (int i = index + 1; i <= TimePeriods.Count - 1; i++) {
                    TimePeriods[i].Order = i - 1;
                    TimePeriods[i].Code = $"{i - 1}".PadLeft(6, '0');
                }
                TimePeriods.Remove(period);
            }
            await Task.CompletedTask;
        }

        private async void SaveTimePeriod(TimePeriodViewModel model) {
           await _dataService.InsertManyAsync(model.TimePeriods.ToList());
        }
        #endregion
    }
}
