using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using XStudio.App.Service;

namespace XStudio.App.ViewModel.Module.Schools {
    public class SectionPageViewModel : ViewModelDataBase<Page> {
        private readonly DataService _dataService;
        private string _type;
        private ObservableCollection<SectionViewModel> _sections;


        public SectionPageViewModel(DataService dataService, string type) {
            _dataService = dataService;
            _type = type;
            DataList = dataService.getSectionPage(this);
            _sections = new ObservableCollection<SectionViewModel>();
            LoadCommand = new DelegateCommand(async () => await LoadDataAsync());
            SaveCommand = new DelegateCommand<SectionPageViewModel>(SaveSection);
            AddCommand = new DelegateCommand(AddSection);
            UpCommand = new DelegateCommand<SectionViewModel>(MoveUp);
            DownCommand = new DelegateCommand<SectionViewModel>(MoveDown);
            DeleteCommand = new DelegateCommand<SectionViewModel>(RemoveSection);
        }

        public async Task LoadDataAsync() {
            IsLoading = true;
            _sections.Clear();
            var data = await _dataService.GetSectionListAsync(new Abp.Application.Services.Dto.PagedAndSortedResultRequestDto() { MaxResultCount = 100, SkipCount = 0, Sorting = "Order" });
            if (data != null && data.Items.Any()) {
                _sections.AddRange(data.Items);
            }
            IsLoading = false;
        }



        public string @Type {
            get => _type;
            set => SetProperty(ref _type, value);
        }
        public ObservableCollection<SectionViewModel> Sections {
            get { return _sections; }
            set { SetProperty(ref _sections, value); }
        }

        #region Commands
        public DelegateCommand<SectionPageViewModel> SaveCommand { get; private set; }

        public DelegateCommand LoadCommand { get; private set; }
        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand<SectionViewModel> UpCommand { get; private set; }
        public DelegateCommand<SectionViewModel> DownCommand { get; private set; }
        public DelegateCommand<SectionViewModel> DeleteCommand { get; private set; }

        private async void AddSection() {
            //var dialog = new TimePeriodWindow();
            //dialog.Owner = System.Windows.Application.Current.MainWindow;
            //dialog.SetOrder(TimePeriods.Any() ? TimePeriods.Max(x => x.Order) + 1 : 0);
            //if (dialog.ShowDialog() == true) {
            //    TimePeriods.Add(dialog.TimePeriodModel);
            //}
            await Task.CompletedTask;
        }

        private void MoveUp(SectionViewModel timePeriod) {
            int index = Sections.IndexOf(timePeriod);
            if (index > 0) {
                // 交换当前项和上一个项的 Order 和 Code
                var previousItem = Sections[index - 1];

                // 交换 Order
                int tempOrder = timePeriod.Order;
                timePeriod.Order = previousItem.Order;
                previousItem.Order = tempOrder;

                // 交换 Code
                string tempCode = timePeriod.Code;
                timePeriod.Code = previousItem.Code;
                previousItem.Code = tempCode;

                // 移动项
                Sections.Move(index, index - 1);
            }
        }

        private void MoveDown(SectionViewModel timePeriod) {
            int index = Sections.IndexOf(timePeriod);
            if (index < Sections.Count - 1) {
                // 交换当前项和下一个项的 Order 和 Code
                var nextItem = Sections[index + 1];

                // 交换 Order
                int tempOrder = timePeriod.Order;
                timePeriod.Order = nextItem.Order;
                nextItem.Order = tempOrder;

                // 交换 Code
                string tempCode = timePeriod.Code;
                timePeriod.Code = nextItem.Code;
                nextItem.Code = tempCode;

                // 移动项
                Sections.Move(index, index + 1);
            }
        }

        private async void RemoveSection(SectionViewModel period) {
            if (period != null) {
                int index = Sections.IndexOf(period);
                for (int i = index + 1; i <= Sections.Count - 1; i++) {
                    Sections[i].Order = i - 1;
                    Sections[i].Code = $"{i - 1}".PadLeft(6, '0');
                }
                Sections.Remove(period);
            }
            await Task.CompletedTask;
        }

        private async void SaveSection(SectionPageViewModel model) {
            await _dataService.InsertManySectionAsync(model.Sections.ToList());
        }
        #endregion
    }
}
