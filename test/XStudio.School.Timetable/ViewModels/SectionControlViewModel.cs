using ImTools;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using XStudio.School.Timetable.Models;
using XStudio.SchoolSchedule;
using XStudio.SchoolSchedule.Rules;
using DayOfWeek = XStudio.SchoolSchedule.DayOfWeek;

namespace XStudio.School.Timetable.ViewModels {
    /// <summary>
    /// 节次管理
    /// </summary>
	public class SectionControlViewModel : MenuItemViewModel {
        private readonly IDialogCoordinator _dialogCoordinator;
        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand<TimetableRow> UpCommand { get; private set; }
        public DelegateCommand<TimetableRow> DownCommand { get; private set; }
        public DelegateCommand<TimetableRow> DeleteCommand { get; private set; }

        public ObservableCollection<DayOfWeek> LayoutOfWeek { get; set; } = new ObservableCollection<DayOfWeek>() {
             DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
             DayOfWeek.Friday, DayOfWeek.Saturday,DayOfWeek.Sunday
        };
        /// <summary>
        /// 节次内容
        /// </summary>
        public ObservableCollection<TimetableRow> TimetableRows { get; set; } = new ObservableCollection<TimetableRow>();
        public ObservableCollection<ObservableCollection<Section>> Sections { get; set; }

        public SectionControlViewModel(IDialogCoordinator dialogCoordinator, HamburgerMenuControlViewModel viewModel)
            : base(viewModel) {
            _dialogCoordinator = dialogCoordinator;
            AddCommand = new DelegateCommand(AddSection);
            UpCommand = new DelegateCommand<TimetableRow>(MoveUp);
            DownCommand = new DelegateCommand<TimetableRow>(MoveDown);
            DeleteCommand = new DelegateCommand<TimetableRow>(RemoveSection);
            Sections = new ObservableCollection<ObservableCollection<Section>>();
            // 获取所有星期
            ClassSchedule classSchedule = GenerateTimetable();
            List<Section> SectionList = classSchedule.Sections;
            for (int i = 1; i <= classSchedule.MaxPeriod; i++) {
                var sectionsForDay = new ObservableCollection<Section>(SectionList.Where(s => s.Period == i));
                Sections.Add(sectionsForDay);
            }
            ConvertToDataTable(classSchedule);
        }

        private void ConvertToDataTable(ClassSchedule classSchedule) {
            TimetableRows.Clear();
        }

        private async void AddSection() {
            //var dialog = new TimePeriodWindow(_dialogCoordinator);
            //dialog.Owner = System.Windows.Application.Current.MainWindow;
            //dialog.SetOrder(TimePeriods.Any() ? TimePeriods.Max(x => x.Order) + 1 : 0);
            //if (dialog.ShowDialog() == true) {
            //    TimePeriods.Add(dialog.TimePeriodModel.TimePeriod);
            //}
            int Period = 1;
            string TimeSlot = "早晨";
            List<string> timePeriods = new List<string>() { "早晨", "上午", "中午", "下午", "晚上" };
            if (TimetableRows.Any()) {
                Period = TimetableRows.Max(x => x.Period) + 1;
                TimeSlot = TimetableRows.FindFirst(x => x.Period == Period - 1)?.TimeSlot;
            }
            TimetableRows.Add(new TimetableRow() { ColSpan = 1, RowSpan = 1, Period = Period, TimeSlot = TimeSlot });
            await Task.CompletedTask;
        }

        private void MoveUp(TimetableRow row) {
            int index = TimetableRows.IndexOf(row);
            if (index > 0) {
                // 交换当前项和上一个项的 Order 和 Code
                var previousItem = TimetableRows[index - 1];
                // 交换 Order
                int tempOrder = row.Period;
                row.Period = previousItem.Period;
                previousItem.Period = tempOrder;

                // 交换 TimePeriod
                string tempTimePeriod = row.TimeSlot;
                row.TimeSlot = previousItem.TimeSlot;
                previousItem.TimeSlot = tempTimePeriod;

                // 移动项
                TimetableRows.Move(index, index - 1);

            }
        }

        private void MoveDown(TimetableRow row) {
            int index = TimetableRows.IndexOf(row);
            if (index < TimetableRows.Count - 1) {
                // 交换当前项和下一个项的 Order 和 Code
                var nextItem = TimetableRows[index + 1];

                // 交换 Order
                int tempOrder = row.Period;
                row.Period = nextItem.Period;
                nextItem.Period = tempOrder;

                // 交换 TimePeriod
                string tempTimePeriod = row.TimeSlot;
                row.TimeSlot = nextItem.TimeSlot;
                nextItem.TimeSlot = tempTimePeriod;

                // 移动项
                TimetableRows.Move(index, index + 1);
            }
        }

        private async void RemoveSection(TimetableRow row) {
            if (TimetableRows != null) {
                TimetableRows.Remove(row);
            }
            await Task.CompletedTask;
        }

        private ClassSchedule GenerateTimetable() {
            ClassSchedule classSchedule = new ClassSchedule();
            // 设置每周的上课时间和表格样式
            classSchedule.LayoutOfWeek = new List<DayOfWeek>() {
             DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
             DayOfWeek.Friday, DayOfWeek.Saturday };
            // 设置节次 16 * 7 = 112 课时; 16 * 40 = 640 分钟
            classSchedule.InitializeSchedule(16);
            // 设置时段, 节次属性
            classSchedule.SetSectionTimePeriod(new List<int>() { 1, 2 }, "早晨", SectionType.MorningStudy); // 14
            classSchedule.SetSectionTimePeriod(new List<int>() { 3, 4 }, "上午", SectionType.RegularClass);
            classSchedule.SetSectionTimePeriod(new List<int>() { 5 }, "上午", SectionType.BreakExercise);
            classSchedule.SetSectionTimePeriod(new List<int>() { 6, 7 }, "上午", SectionType.RegularClass); // 35 = 49
            classSchedule.SetSectionTimePeriod(new List<int>() { 8 }, "中午", SectionType.NoonBreak);
            classSchedule.SetSectionTimePeriod(new List<int>() { 9 }, "中午", SectionType.AfternoonStudy);    // 14 = 63
            classSchedule.SetSectionTimePeriod(new List<int>() { 10, 11, 12, 13 }, "下午", SectionType.RegularClass); // 35 = 98
            classSchedule.SetSectionTimePeriod(new List<int>() { 14, 15, 16 }, "晚上", SectionType.EveningStudy);     // 21 = 119
                                                                                                                    // 设置通栏即合并单元格 - 不参与排课与自动排课
            classSchedule.SetColumnSpan(DayOfWeek.Monday, 5, 7); // 设置第5|8|9节从周1到周日合并单元格(通栏) - 不参与排课与自动排课
            classSchedule.SetColumnSpan(DayOfWeek.Monday, 8, 7);
            classSchedule.SetColumnSpan(DayOfWeek.Monday, 9, 7);
            // 排课 总课时: 16 * 7 = 112; 早读: 2 * 7 = 14 课时; 正课: 8 * 7 = 56 课时; 晚自习: 3 * 7 = 21 课时; 课间操: 1 * 7 = 7 课时; 午休: 2 * 7 = 14 课时;
            List<string> ClassCourseList = new() { "语文", "数学", "英语", "物理", "化学", "生物", "历史", "地理", "政治", "公共课", "体育", "美术", "音乐", "舞蹈", "戏剧", "电影", "健康课", "心理课", "综合课" };
            Dictionary<string, ClassCourseRule> classCourses = ClassCourseList.ToDictionary(k => k, v => new ClassCourseRule() { Name = v, Mode = RuleMode.Course, Priority = PriorityMode.Medium, Type = RuleType.None });
            //分解课时

            /* 自动排课 - 默认校验:只校验班级课程课时，
             *            选择校验：
             *              1.年级课程冲突；
             *              2.教师课时；
             *              3.教师课程冲突；
             */
            classSchedule.AddSectionContent("00101", new SectionContent(0, new ClassCourseRule() { Name = "语文", Mode = RuleMode.Course, Priority = 0, Type = RuleType.None }));
            classSchedule.AddSectionContent("00105", new SectionContent(0, new ClassCourseRule() { Name = "眼保健操", Type = RuleType.Unknown }));
            ClassCourseRule courseRule1 = new() { Name = "语文", Mode = RuleMode.Course, Priority = 0, Type = RuleType.Single };
            ClassCourseRule courseRule2 = new() { Name = "数学", Mode = RuleMode.Course, Priority = 0, Type = RuleType.Biweekly };
            classSchedule.AddSectionContent("00106", new SectionContent(0, new SingleOrBiweeklyRule(PriorityMode.Highest, courseRule1, courseRule2)));
            return classSchedule;
        }
    }
}
