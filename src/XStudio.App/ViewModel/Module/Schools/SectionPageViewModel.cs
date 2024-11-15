using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using XStudio.App.Helper;
using XStudio.App.Service;
using XStudio.App.Views.Module.Schools;

namespace XStudio.App.ViewModel.Module.Schools {
    public class ScheduleSectionPageViewModel : ViewModelDataBase<Page> {
        private readonly DataService _dataService;
        private string _type = string.Empty;
        private ObservableCollection<ScheduleViewModel> _schedules;
        private ObservableCollection<SectionViewModel> _sections;
        private ScheduleViewModel? _selectedSchedule;

        public ScheduleSectionPageViewModel(DataService dataService, string type) {
            _dataService = dataService;
            _type = type;
            DataList = dataService.getSchedulePage(this);
            _schedules = new ObservableCollection<ScheduleViewModel>();
            _sections = new ObservableCollection<SectionViewModel>();
            LoadCommand = new DelegateCommand(async () => await LoadSectionDataAsync());

            AddScheduleCommand = new DelegateCommand(AddSchedule);
            DeleteScheduleCommand = new DelegateCommand<ScheduleViewModel>(RemoveSchedule);
            SelectedScheduleCommand = new DelegateCommand<ScheduleViewModel>(SelectedScheduleAction);

            AddSectionCommand = new DelegateCommand(AddSection);
            UpSectionCommand = new DelegateCommand<SectionViewModel>(MoveUpSection);
            DownSectionCommand = new DelegateCommand<SectionViewModel>(MoveDownSection);
            DeleteSectionCommand = new DelegateCommand<SectionViewModel>(RemoveSection);
            SaveSectionCommand = new DelegateCommand<ScheduleSectionPageViewModel>(SaveSection);
        }

        public async Task LoadScheduleDataAsync() {
            
            IsLoading = true;
            _schedules.Clear();
            var scheduleList = await _dataService.GetScheduleListAsync(new Abp.Application.Services.Dto.PagedAndSortedResultRequestDto() { MaxResultCount = 100, SkipCount = 0, Sorting = "Order" });
            if (scheduleList != null && scheduleList.Items.Any()) {
                _schedules.AddRange(scheduleList.Items);
            }
            IsLoading = false;
        }

        public async Task LoadSectionDataAsync() {
            if (_selectedSchedule == null) {
                return;
            }
            IsLoading = true;
            _sections.Clear();
            var data = await _dataService.GetSectionListAsync(_selectedSchedule.Code, new Abp.Application.Services.Dto.PagedAndSortedResultRequestDto() { MaxResultCount = 100, SkipCount = 0, Sorting = "Order" });
            if (data != null && data.Items.Any()) {
                _sections.AddRange(data.Items);
            }
            IsLoading = false;
        }

        public string @Type {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        public ObservableCollection<ScheduleViewModel> Schedules {
            get { return _schedules; }
            set { SetProperty(ref _schedules, value); }
        }

        public ScheduleViewModel? SelectedSchedule { 
            get => _selectedSchedule;
            set => SetProperty(ref _selectedSchedule, value);
        }

        public ObservableCollection<SectionViewModel> Sections {
            get { return _sections; }
            set { SetProperty(ref _sections, value); }
        }

        #region Commands

        public DelegateCommand LoadCommand { get; private set; }

        #endregion
        #region 节次方案
        public DelegateCommand AddScheduleCommand { get; private set; }
        public DelegateCommand<ScheduleViewModel> DeleteScheduleCommand { get; private set; }
        public DelegateCommand<ScheduleViewModel> SelectedScheduleCommand { get; private set; }

        private async void SelectedScheduleAction(ScheduleViewModel schedule) {
            if (schedule != null) {
                SelectedSchedule = schedule;
                await LoadSectionDataAsync();
            }
        }

        private async void RemoveSchedule(ScheduleViewModel schedule) {
            if (schedule != null) {
                int index = Schedules.IndexOf(schedule);
                for (int i = index + 1; i <= Schedules.Count - 1; i++) {
                    Schedules[i].Order = i - 1;
                    Schedules[i].Code = $"{i - 1}".PadLeft(6, '0');
                }
                Schedules.Remove(schedule);
            }
            await Task.CompletedTask;
        }

        private async void AddSchedule() {
            var dialog = new ScheduleWindow();
            dialog.Owner = System.Windows.Application.Current.MainWindow;
            dialog.SetOrder(Schedules.Any() ? Schedules.Max(x => x.Order) + 1 : 0);
            if (dialog.ShowDialog() == true) {
                Schedules.Add(dialog.ScheduleModel);
            }
            await Task.CompletedTask;
        }

        #endregion
        #region 节次
        public DelegateCommand AddSectionCommand { get; private set; }
        public DelegateCommand<SectionViewModel> UpSectionCommand { get; private set; }
        public DelegateCommand<SectionViewModel> DownSectionCommand { get; private set; }
        public DelegateCommand<SectionViewModel> DeleteSectionCommand { get; private set; }
        public DelegateCommand<ScheduleSectionPageViewModel> SaveSectionCommand { get; private set; }

        private async void AddSection() {
            int order = Sections.Any() ? Sections.Max(x => x.Order) + 1 : 0;
            Sections.Add(new SectionViewModel() { Order = order, Code = order.GenerateCode(6) });
            await Task.CompletedTask;
        }

        private void MoveUpSection(SectionViewModel timePeriod) {
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

        private void MoveDownSection(SectionViewModel timePeriod) {
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

        private async void SaveSection(ScheduleSectionPageViewModel model) {
            await _dataService.InsertManySectionAsync(model.Sections.ToList());
        }
        #endregion
    }
}
