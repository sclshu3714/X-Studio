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
    public class TimePeriodPageViewModel : ViewModelDataBase<Page> {
        private readonly DataService _dataService;
        private string _type;
        private ObservableCollection<TimePeriodViewModel> _timePeriods;
        

        public TimePeriodPageViewModel(DataService dataService,string type) {
            _dataService = dataService;
            _type = type;
            DataList = dataService.getTimePeriodPage(this);
            _timePeriods = new ObservableCollection<TimePeriodViewModel>();
            LoadCommand = new DelegateCommand(async () => await LoadDataAsync());
            SaveCommand = new DelegateCommand<TimePeriodPageViewModel>(SaveTimePeriod);
            AddCommand = new DelegateCommand(AddTimePeriod);
            UpCommand = new DelegateCommand<TimePeriodViewModel>(MoveUp);
            DownCommand = new DelegateCommand<TimePeriodViewModel>(MoveDown);
            DeleteCommand = new DelegateCommand<TimePeriodViewModel>(RemoveTimePeriod);
        }

        public async Task LoadDataAsync() {
            IsLoading = true;
            TimePeriods.Clear();
            var data = await _dataService.GetListAsync(new Abp.Application.Services.Dto.PagedAndSortedResultRequestDto() { MaxResultCount = 100, SkipCount = 0, Sorting = "Order" });
            if (data != null && data.Items.Any()) {
                TimePeriods.AddRange(data.Items);
            }
            IsLoading = false;
        }

        

        public string @Type {
            get => _type;
            set => SetProperty(ref _type, value);
        }
        public ObservableCollection<TimePeriodViewModel> TimePeriods {
            get { return _timePeriods; }
            set { SetProperty(ref _timePeriods, value); }
        }

        #region Commands
        public DelegateCommand<TimePeriodPageViewModel> SaveCommand { get; private set; }

        public DelegateCommand LoadCommand { get; private set; }
        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand<TimePeriodViewModel> UpCommand { get; private set; }
        public DelegateCommand<TimePeriodViewModel> DownCommand { get; private set; }
        public DelegateCommand<TimePeriodViewModel> DeleteCommand { get; private set; }

        private async void AddTimePeriod() {
            var dialog = new TimePeriodWindow();
            dialog.Owner = System.Windows.Application.Current.MainWindow;
            dialog.SetOrder(TimePeriods.Any() ? TimePeriods.Max(x => x.Order) + 1 : 0);
            if (dialog.ShowDialog() == true) {
                TimePeriods.Add(dialog.TimePeriodModel);
            }
            await Task.CompletedTask;
        }

        private void MoveUp(TimePeriodViewModel timePeriod) {
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

        private void MoveDown(TimePeriodViewModel timePeriod) {
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

        private async void RemoveTimePeriod(TimePeriodViewModel period) {
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

        private async void SaveTimePeriod(TimePeriodPageViewModel model) {
           await _dataService.InsertManyAsync(model.TimePeriods.ToList());
        }
        #endregion
    }
}
