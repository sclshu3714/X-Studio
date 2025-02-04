using ImTools;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml;
using XStudio.School.Timetable.Models;
using XStudio.SchoolSchedule;
using XStudio.SchoolSchedule.Algorithms;
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
        public DelegateCommand<object> ReGenerateCommand { get; private set; }
        public ClassSchedule classSchedule { get; set; }

        private HeuristicScheduler algorithm = new HeuristicScheduler();

        public ObservableCollection<DayOfWeek> LayoutOfWeek { get; set; } = new ObservableCollection<DayOfWeek>() {
             DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
             DayOfWeek.Friday, DayOfWeek.Saturday,DayOfWeek.Sunday
        };
        /// <summary>
        /// 节次内容
        /// </summary>
        public ObservableCollection<TimetableRow> TimetableRows { get; set; } = new ObservableCollection<TimetableRow>();
        //public ObservableCollection<ObservableCollection<Section>> Sections { get; set; }

        public SectionControlViewModel(IDialogCoordinator dialogCoordinator, HamburgerMenuControlViewModel viewModel)
            : base(viewModel) {
            _dialogCoordinator = dialogCoordinator;
            AddCommand = new DelegateCommand(AddSection);
            UpCommand = new DelegateCommand<TimetableRow>(MoveUp);
            DownCommand = new DelegateCommand<TimetableRow>(MoveDown);
            DeleteCommand = new DelegateCommand<TimetableRow>(RemoveSection);
            ReGenerateCommand = new DelegateCommand<object>(ReGenerateSection);
            //Sections = new ObservableCollection<ObservableCollection<Section>>();
            // 获取所有星期
            classSchedule = GenerateTimetable();
            Console.Write(JsonConvert.SerializeObject(classSchedule));
            //List<Section> SectionList = classSchedule.Sections;
            //for (int i = 1; i <= classSchedule.MaxPeriod; i++) {
            //    var sectionsForDay = new ObservableCollection<Section>(SectionList.Where(s => s.Period == i));
            //    Sections.Add(sectionsForDay);
            //}
            ConvertToDataTable(classSchedule);
        }

        private void ReGenerateSection(object row) {
            SectionControlViewModel sectionControlViewModel = (SectionControlViewModel)row;
            sectionControlViewModel.classSchedule = GenerateTimetable();
            Console.Write(JsonConvert.SerializeObject(classSchedule));
            ConvertToDataTable(sectionControlViewModel.classSchedule);
        }

        private void ConvertToDataTable(ClassSchedule classSchedule) {
            TimetableRows.Clear();
            List<Section> SectionList = classSchedule.Sections;
            for(int i = 1; i <= classSchedule.MaxPeriod; i++) {
                var sectionsForDay = new ObservableCollection<Section>(SectionList.Where(s => s.Period == i));
                if(!sectionsForDay.Any())
                    continue;
                // 构造 TimetableRow
                Section tempSection = sectionsForDay.FirstOrDefault();
                TimetableRow timetable = new TimetableRow() {
                    TimeSlot = tempSection.TimePeriod,
                    Period = i
                };
                foreach(var section in sectionsForDay) {
                    Brush dayForeground = Brushes.Black;
                    Brush dayBackground = Brushes.White;
                    string displayName = section.Contents.FirstOrDefault()?.Content?.DisplayName ?? "";
                    TimetableCell cell = new TimetableCell() {
                        Row = timetable,
                        Column = classSchedule.LayoutOfWeek.IndexOf(section.Day),
                        Day = section.Day,
                        Content = displayName,
                    };
                    if(section.IsMergeCell && section.LinkTo == null) {
                        cell.IsMerged = section.IsMergeCell;
                        cell.ColSpan = section.ColSpan;
                        cell.RowSpan = section.RowSpan;
                    }
                    if(SetDayCellBrush(section.Contents.FirstOrDefault()?.Content?.Type, ref dayForeground, ref dayBackground)) {
                        cell.Foreground = dayForeground;
                        cell.Background = dayBackground;
                    }
                    //timetable.Cells.Add(cell);
                    var propertyDayInfo = timetable.GetType().GetProperty($"{section.Day}");
                    if(propertyDayInfo != null) {
                        propertyDayInfo.SetValue(timetable, cell);
                    }
                }
                timetable.RowSpan = 3;
                TimetableRows.Add(timetable);
            }
        }

        private bool SetDayCellBrush(RuleType? type, ref Brush dayForeground, ref Brush dayBackground) {
            switch(type) {
                case RuleType.ConsecutiveClasses:
                    dayForeground = Brushes.Blue;
                    dayBackground = Brushes.LightGray;
                    break;
                case RuleType.AlternatePolling:
                    dayForeground = Brushes.BlueViolet;
                    dayBackground = Brushes.LightGray;
                    break;
                case RuleType.SingleOrBiweekly:
                    dayForeground = Brushes.CadetBlue;
                    dayBackground = Brushes.LightGray;
                    break;
                case RuleType.CanOnlyArrange:
                    dayForeground = Brushes.DarkGreen;
                    dayBackground = Brushes.LightGray;
                    break;
                default:
                    dayForeground = Brushes.Black;
                    dayBackground = Brushes.White;
                    break;
            }
            return true;
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
            if(TimetableRows.Any()) {
                Period = TimetableRows.Max(x => x.Period) + 1;
                TimeSlot = TimetableRows.FindFirst(x => x.Period == Period - 1)?.TimeSlot;
            }
            TimetableRows.Add(new TimetableRow() { Period = Period, TimeSlot = TimeSlot, Cells = new ObservableCollection<TimetableCell>() });
            await Task.CompletedTask;
        }

        private void MoveUp(TimetableRow row) {
            int index = TimetableRows.IndexOf(row);
            if(index > 0) {
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
            if(index < TimetableRows.Count - 1) {
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
            if(TimetableRows != null) {
                TimetableRows.Remove(row);
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// 一个班级生成课表
        /// </summary>
        /// <returns></returns>
        private ClassSchedule GenerateTimetable() {
            ClassSchedule classSchedule = new ClassSchedule();
            // 设置每周的上课时间和表格样式
            classSchedule.LayoutOfWeek = new List<DayOfWeek>() {
             DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
             DayOfWeek.Friday, DayOfWeek.Saturday };
            // 设置节次 16 * 7 = 112 课时; 16 * 40 = 640 分钟
            classSchedule.InitializeSchedule(16);
            // 设置时段, 节次属性
            SetTimePeriods(classSchedule);
            // 设置通栏即合并单元格 - 不参与排课与自动排课
            classSchedule.SetColumnSpan(DayOfWeek.Monday, 5, 7); // 设置第5|8|9节从周1到周日合并单元格(通栏) - 不参与排课与自动排课
            classSchedule.SetColumnSpan(DayOfWeek.Monday, 8, 7);
            classSchedule.SetColumnSpan(DayOfWeek.Monday, 9, 7);
            // 排课 总课时: 16 * 7 = 112; 早读: 2 * 7 = 14 课时; 正课: 8 * 7 = 56 课时; 晚自习: 3 * 7 = 21 课时; 课间操: 1 * 7 = 7 课时; 午休: 2 * 7 = 14 课时;
            List<ClassCourse> classCourses = DefaultClassCourseList();
            Dictionary<string, ClassCourse> pairs = classCourses.ToDictionary(k => k.Name, v => v);
            Dictionary<string, ClassCourseRule> topicRules = ToClassCourseRules(pairs);
            List<IRule> rules = DefaultRules(pairs);
            List<IRule> constraint = DefaultConstraint(topicRules);
            NoAssignCourses = null;
            algorithm.StartAutoAssignCourses(SchedulerType.Genetic, classSchedule, rules, constraint);
            NoAssignCourses = algorithm.NoAssignCourses;
            //AutoAssignCourses(classSchedule, rules, constraint, 0);
            Console.Write(NoAssignCourses);
            //分解课时 
            /* 自动排课 - 默认校验:只校验班级课程课时，
             *            选择校验：
             *              1.年级课程冲突；
             *              2.教师课时；
             *              3.教师课程冲突；
             */
            //classSchedule.AddSectionContent("00101", new SectionContent(0, new ClassCourseRule() { Name = "语文", Mode = RuleMode.Course, Priority = 0, Type = RuleType.None }));
            //classSchedule.AddSectionContent("00105", new SectionContent(0, new ClassCourseRule() { Name = "眼保健操", Type = RuleType.Unknown }));
            //ClassCourseRule courseRule1 = new() { Name = "语文", Mode = RuleMode.Course, Priority = 0, Type = RuleType.Single };
            //ClassCourseRule courseRule2 = new() { Name = "数学", Mode = RuleMode.Course, Priority = 0, Type = RuleType.Biweekly };
            //classSchedule.AddSectionContent("00106", new SectionContent(0, new SingleOrBiweeklyRule(PriorityMode.Highest, courseRule1, courseRule2)));
            return classSchedule;
        }

        /// <summary>
        /// 生成多个班级课表
        /// </summary>
        /// <returns></returns>
        private List<ClassSchedule> GenerateTimetables() {
            // 示例数据，生成一个年级12个班级的课表
            List<ClassSchedule> classSchedules = new();
            for(int i = 1; i <= 12; i++) {
                ClassSchedule classSchedule = new ClassSchedule();
                // 设置每周的上课时间和表格样式
                classSchedule.LayoutOfWeek = new List<DayOfWeek>() {
                     DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
                     DayOfWeek.Friday, DayOfWeek.Saturday
                };
                // 设置节次 16 * 7 = 112 课时; 16 * 40 = 640 分钟
                classSchedule.InitializeSchedule(16);
                // 设置时段, 节次属性
                SetTimePeriods(classSchedule);
                // 设置通栏即合并单元格 - 不参与排课与自动排课
                classSchedule.SetColumnSpan(DayOfWeek.Monday, 5, 7); // 设置第5|8|9节从周1到周日合并单元格(通栏) - 不参与排课与自动排课
                classSchedule.SetColumnSpan(DayOfWeek.Monday, 8, 7);
                classSchedule.SetColumnSpan(DayOfWeek.Monday, 9, 7);
                List<ClassCourse> classCourses = DefaultClassCourseList();
                Dictionary<string, ClassCourse> pairs = classCourses.ToDictionary(k => k.Name, v => v);
                Dictionary<string, ClassCourseRule> topicRules = ToClassCourseRules(pairs);
                List<IRule> rules = DefaultRules(pairs);
                List<IRule> constraint = DefaultConstraint(topicRules);
                NoAssignCourses = null;
                algorithm.StartAutoAssignCourses(SchedulerType.Genetic, classSchedule, rules, constraint);
                NoAssignCourses = algorithm.NoAssignCourses;
                Console.Write(NoAssignCourses);
                classSchedules.Add(classSchedule);
            }
            return classSchedules;
        }

        private Dictionary<string, ClassCourseRule> ToClassCourseRules(Dictionary<string, ClassCourse> pairs) {
            Dictionary<string, ClassCourseRule> keyValuePairs = new();
            foreach(var item in pairs) {
                ClassCourse course = item.Value;
                keyValuePairs.Add($"{course.Name}(早)", new ClassCourseRule() {
                    Name = $"{course.Name}(早)",
                    Code = course.Code,
                    Priority = PriorityMode.Medium,
                });
                keyValuePairs.Add($"{course.Name}", new ClassCourseRule() {
                    Name = course.Name,
                    Code = course.Code,
                    Priority = PriorityMode.Medium,
                });
                keyValuePairs.Add($"{course.Name}(晚)", new ClassCourseRule() {
                    Name = $"{course.Name}(晚)",
                    Code = course.Code,
                    Priority = PriorityMode.Medium,
                });
            }
            return keyValuePairs;
        }

        public List<ClassCourse> DefaultClassCourseList() {
            // 排课 总课时: 16 * 7 = 112; 早读: 2 * 7 = 14 课时; 正课: 8 * 7 = 56 课时; 晚自习: 3 * 7 = 21 课时; 课间操: 1 * 7 = 7 课时; 午休: 2 * 7 = 14 课时;
            List<ClassCourse> ClassCourseList = new()
            {
                new ClassCourse("XXXQ0101001","语文", 6.0f),
                new ClassCourse("XXXQ0101002","数学", 6.0f),
                new ClassCourse("XXXQ0101003","英语", 6.0f),
                new ClassCourse("XXXQ0101004","物理", 4.5f),
                new ClassCourse("XXXQ0101005","化学", 4.5f),
                new ClassCourse("XXXQ0101006","生物", 4.5f),
                new ClassCourse("XXXQ0101007","历史", 4.5f),
                new ClassCourse("XXXQ0101008","地理", 4.5f),
                new ClassCourse("XXXQ0101009","政治", 4.5f),
                new ClassCourse("XXXQ0101010","体育", 1.0f),
                new ClassCourse("XXXQ0101011","美术", 0.5f),
                new ClassCourse("XXXQ0101012","音乐", 0.5f),
                new ClassCourse("XXXQ0101013","班会", 1.0f),
                new ClassCourse("XXXQ0101014","舞蹈", 0.1f),
                new ClassCourse("XXXQ0101015","戏剧", 0.1f),
                new ClassCourse("XXXQ0101016","电影", 0.1f),
                new ClassCourse("XXXQ0101017","健康", 0.1f),
                new ClassCourse("XXXQ0101018","心理", 0.1f),
                new ClassCourse("XXXQ0101019","综合", 0.1f)
            };
            return ClassCourseList;
        }

        /// <summary>
        /// 默认约束
        /// </summary>
        /// <param name="classCourses"></param>
        /// <returns></returns>
        private List<IRule> DefaultConstraint(Dictionary<string, ClassCourseRule> classCourses) {
            List<IRule> constraint = new() {
                new CanOnlyArrange(PriorityMode.Highest, RuleMode.Course, classCourses["语文"], Tuple.Create(DayOfWeek.Monday, 3)),
                new CanOnlyArrange(PriorityMode.Highest, RuleMode.Course, classCourses["数学"], Tuple.Create(DayOfWeek.Wednesday, 3)),
                new CannotBeArranged(PriorityMode.Highest, RuleMode.Course, classCourses["数学"], Tuple.Create(DayOfWeek.Thursday, 3))
            };
            return constraint;
        }

        /// <summary>
        /// 默认课程()
        /// </summary>
        /// <param name="classCourses"></param>
        /// <returns></returns>
        private List<IRule> DefaultRules(Dictionary<string, ClassCourse> topicRules) {
            // 排课 总课时: 16 * 7 = 112; 早读: 2 * 7 = 14 课时; 正课: 8 * 7 = 56 课时; 晚自习: 3 * 7 = 21 课时; 课间操: 1 * 7 = 7 课时; 午休: 2 * 7 = 14 课时;
            // 构建测试规则
            // 1 语文 7 课时
            // 2 数学 7 课时
            // 3 英语 7 课时                                     -> 21 课时
            // 4 物理 5.5 课时
            // 5 化学 5.5 课时
            // 6 生物 5.5 课时 
            // 7 历史 5.5 课时
            // 8 地理 4.5 课时
            // 9 政治 4.5 课时                                    -> 31 课时
            // 10 体育 1 课时
            // 11 美术 0.5 课时
            // 12 音乐 0.5 课时
            // 13 舞蹈、戏剧, 电影, 健康课, 心理课, 综合课 1 课时 -> 3
            // 14 班会 1 课时                                     -> 1

            List<IRule> rules = new();
            //总课时: 16 * 7 = 112; 早读: 2 * 7 = 14 课时; 正课: 8 * 7 = 56 课时; 晚自习: 3 * 7 = 21 课时; 课间操: 1 * 7 = 7 课时; 午休: 2 * 7 = 14
            //已经使用课时：
            //  早自习：语文    数学    英语    物理    化学    生物    历史    地理    政治    体育    美术    音乐    舞蹈    戏剧    电影    健康    心理    综合
            //           2       2       2       0       0       0       0       0       0       0       0       0       0       0       0       0       0       0
            //  正  课：语文    数学    英语    物理    化学    生物    历史    地理    政治    体育    美术    音乐    舞蹈    戏剧    电影    健康    心理    综合
            //           0       0       0       0       0        0      0       0       0       0       0       0       0       0       0       0       0       0
            //  晚自习：语文    数学    英语    物理    化学    生物    历史    地理    政治    体育    美术    音乐    舞蹈    戏剧    电影    健康    心理    综合
            //           0       0       0       0       0       0       0       0       0       0       0       0       0       0       0       0       0       0
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["语文"] }, RuleType.ConsecutiveClasses, SectionType.MorningStudy, 1, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["数学"] }, RuleType.ConsecutiveClasses, SectionType.MorningStudy, 1, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["数学"] }, RuleType.ConsecutiveClasses, SectionType.MorningStudy, 1, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["语文"] }, RuleType.ConsecutiveClasses, SectionType.RegularClass, 1, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["数学"] }, RuleType.ConsecutiveClasses, SectionType.RegularClass, 1, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["英语"] }, RuleType.ConsecutiveClasses, SectionType.RegularClass, 1, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["语文"] }, RuleType.ConsecutiveClasses, SectionType.EveningStudy, 1, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["物理"], topicRules["政治"] }, RuleType.SingleOrBiweekly, SectionType.RegularClass, 1, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["化学"], topicRules["生物"] }, RuleType.SingleOrBiweekly, SectionType.RegularClass, 1, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["历史"], topicRules["地理"] }, RuleType.SingleOrBiweekly, SectionType.RegularClass, 1, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["美术"], topicRules["音乐"] }, RuleType.SingleOrBiweekly, SectionType.RegularClass, 1, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["舞蹈"], topicRules["戏剧"], topicRules["电影"], topicRules["健康"], topicRules["心理"], topicRules["综合"] }, RuleType.AlternatePolling, SectionType.RegularClass, 1, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["语文"] }, RuleType.None, SectionType.RegularClass, 5, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["数学"] }, RuleType.None, SectionType.RegularClass, 5, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["英语"] }, RuleType.None, SectionType.RegularClass, 5, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["物理"] }, RuleType.None, SectionType.RegularClass, 6, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["化学"] }, RuleType.None, SectionType.RegularClass, 6, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["生物"] }, RuleType.None, SectionType.RegularClass, 6, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["历史"] }, RuleType.None, SectionType.RegularClass, 6, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["地理"] }, RuleType.None, SectionType.RegularClass, 5, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["政治"] }, RuleType.None, SectionType.RegularClass, 6, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["体育"] }, RuleType.None, SectionType.RegularClass, 1, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["班会"] }, RuleType.None, SectionType.RegularClass, 1, PriorityMode.Highest));

            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["物理"] }, RuleType.None, SectionType.MorningStudy, 2, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["化学"] }, RuleType.None, SectionType.MorningStudy, 2, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["生物"] }, RuleType.None, SectionType.MorningStudy, 2, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["政治"] }, RuleType.None, SectionType.MorningStudy, 2, PriorityMode.Highest));

            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["数学"] }, RuleType.None, SectionType.EveningStudy, 2, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["英语"] }, RuleType.None, SectionType.EveningStudy, 2, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["物理"] }, RuleType.None, SectionType.EveningStudy, 2, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["化学"] }, RuleType.None, SectionType.EveningStudy, 2, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["历史"] }, RuleType.None, SectionType.EveningStudy, 2, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["地理"] }, RuleType.None, SectionType.EveningStudy, 2, PriorityMode.Highest));
            rules.AddRange(GetRules(new List<ClassCourse>() { topicRules["语文"] }, RuleType.None, SectionType.EveningStudy, 2, PriorityMode.Highest)); // 多余1个晚自习
            return rules;
        }

        /// <summary>
        /// 课程生成规则
        /// </summary>
        /// <param name="classCourse"></param>
        /// <returns></returns>
        public List<IRule> GetRules(List<ClassCourse> classCourses, RuleType ruleType, SectionType sectionType, int classHour, PriorityMode priority = PriorityMode.Medium) {
            if(!classCourses.Any()) {
                return new List<IRule>();
            }
            List<ClassCourseRule> classCourseRules = classCourses.Select(it => new ClassCourseRule() {
                Name = sectionType == SectionType.MorningStudy ? $"{it.Name}(早)" : (sectionType == SectionType.EveningStudy ? $"{it.Name}(晚)" : it.Name),
                Code = it.Code,
                Priority = priority,
                RestrictType = sectionType,
            }).ToList();
            List<IRule> rules = new();
            for(int i = 0; i < classHour; i++) {
                switch(ruleType) {
                    case RuleType.ConsecutiveClasses:
                        // 连堂课
                        ConsecutiveClasses consecutiveClasses = new ConsecutiveClasses(priority, classCourseRules.First()) { ClassHour = 2 };
                        rules.Add(consecutiveClasses);
                        break;
                    case RuleType.AlternatePolling:
                        // 交替轮换课
                        AlternatePolling alternatePolling = new AlternatePolling(priority, classCourseRules) { ClassHour = 1 };
                        rules.Add(alternatePolling);
                        break;
                    case RuleType.SingleOrBiweekly:
                        // 单双周课
                        SingleOrBiweekly singleOrBiweekly = new SingleOrBiweekly(priority, classCourseRules.First(), classCourseRules.Last()) { ClassHour = 1 };
                        rules.Add(singleOrBiweekly);
                        break;
                    case RuleType.None:
                    default:
                        // 单课
                        rules.Add(classCourseRules.First());
                        break;
                }
            }
            return rules;
        }

        // 设置时间段和节次的函数
        private void SetTimePeriods(ClassSchedule classSchedule) {
            classSchedule.SetSectionTimePeriod(new List<int>() { 1, 2 }, "早晨", SectionType.MorningStudy);
            classSchedule.SetSectionTimePeriod(new List<int>() { 3, 4 }, "上午", SectionType.RegularClass);
            classSchedule.SetSectionTimePeriod(new List<int>() { 5 }, "上午", SectionType.BreakExercise, false);
            classSchedule.SetSectionTimePeriod(new List<int>() { 6, 7 }, "上午", SectionType.RegularClass);
            classSchedule.SetSectionTimePeriod(new List<int>() { 8 }, "中午", SectionType.NoonBreak, false);
            classSchedule.SetSectionTimePeriod(new List<int>() { 9 }, "中午", SectionType.AfternoonStudy);
            classSchedule.SetSectionTimePeriod(new List<int>() { 10, 11, 12, 13 }, "下午", SectionType.RegularClass);
            classSchedule.SetSectionTimePeriod(new List<int>() { 14 }, "晚上", SectionType.RegularClass);
            classSchedule.SetSectionTimePeriod(new List<int>() { 15, 16 }, "晚上", SectionType.EveningStudy);
        }
        private string NoAssignCourses = null;
        /// <summary>
        /// 自动分配课程的函数
        /// </summary>
        /// <param name="classSchedule">课表节次</param>
        /// <param name="courses">规则课程</param>
        /// <param name="constraint">约束</param>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool AutoAssignCourses(ClassSchedule classSchedule, List<IRule> courses, List<IRule> constraint, int index) {
            if(index >= courses.Count) {
                return true; // 所有课程都成功分配
            }

            // 先检查约束, 看看是否存在只能排，如果存在只能排，则优先排只能排的课程
            if(constraint != null && constraint.Count > 0) {

                classSchedule.RunCanOnlyArrange(courses, constraint.Where(x => x.Type == RuleType.CanOnlyArrange));
                constraint = constraint.Where(x => x.Type != RuleType.CanOnlyArrange).ToList();
            }

            IRule rule = courses[index];

            // 遍历所有时段尝试分配
            var section = classSchedule.GetAvailableSections(rule, SectionType.RegularClass);
            Tuple<bool, string> tupleAssign = classSchedule.CanAssign(section, rule, constraint);
            if(tupleAssign.Item1) {
                switch(rule.Type) {
                    case RuleType.ConsecutiveClasses:
                        // 连堂课
                        ConsecutiveClasses continuousClasses = (ConsecutiveClasses)rule;
                        continuousClasses.Periods = new List<int>() { section.Period, section.Period + 1 };
                        classSchedule.AddSectionContent(section.Code, new SectionContent(0, continuousClasses));
                        section = classSchedule[section.Day, section.Period + 1];
                        classSchedule.AddSectionContent(section.Code, new SectionContent(0, continuousClasses));
                        break;
                    case RuleType.AlternatePolling:
                        // 交替轮换课
                        AlternatePolling polling = (AlternatePolling)rule;
                        classSchedule.AddSectionContent(section.Code, new SectionContent(0, polling, polling.PollingCourses.Count - 1));
                        break;
                    case RuleType.SingleOrBiweekly:
                        // 单双周课
                        classSchedule.AddSectionContent(section.Code, new SectionContent(0, rule, 1));
                        break;
                    default:
                        classSchedule.AddSectionContent(section.Code, new SectionContent(0, rule));
                        break;
                }
                // 递归尝试下一个课程
                if(AutoAssignCourses(classSchedule, courses, constraint, index + 1)) {
                    return true; // 找到了有效的课程安排
                }
                NoAssignCourses += rule.DisplayName + " ";
                // 如果失败，撤销分配
                classSchedule.RemoveSectionContent(section.Code);
            }
            NoAssignCourses += rule.DisplayName + " ";
            return false; // 该课程无法分配，返回失败
        }

        /// <summary>
        /// 交换展示课表的两个单元格的函数
        /// </summary>
        /// <param name="draggedCell">拖动的单元格</param>
        /// <param name="targetCell">放置的单元格</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> ExchangeTimetableCellAsync(TimetableCell draggedCell, TimetableCell targetCell) {
            if(draggedCell == null || targetCell == null || (draggedCell.Day == targetCell.Day && draggedCell.Row.Period == targetCell.Row.Period)) {
                return false;
            }

            TimetableRow draggedRow = TimetableRows.FirstOrDefault(x => x.Period == draggedCell.Row.Period);
            if(draggedRow == null) {
                return false;
            }

            TimetableRow targetRow = TimetableRows.FirstOrDefault(x => x.Period == targetCell.Row.Period);
            if(targetRow == null) {
                return false;
            }
            // 交换课表数据
            var draggedCellInfo = draggedRow.GetType().GetProperty($"{draggedCell.Day}");
            var targetCellInfo = targetRow.GetType().GetProperty($"{targetCell.Day}");
            if(targetCellInfo != null && draggedCellInfo != null) {
                TimetableCell theTargetCell = new TimetableCell() {
                    Row = targetRow,
                    Column = targetCell.Column,
                    Day = targetCell.Day,
                    Content = draggedCell.Content,
                    IsMerged = draggedCell.IsMerged,
                    ColSpan = draggedCell.ColSpan,
                    RowSpan = draggedCell.RowSpan,
                    Foreground = draggedCell.Foreground,
                    Background = draggedCell.Background,
                };
                targetCellInfo?.SetValue(targetRow, theTargetCell);

                TimetableCell theDraggedCell = new TimetableCell() {
                    Row = draggedRow,
                    Column = draggedCell.Column,
                    Day = draggedCell.Day,
                    Content = targetCell.Content,
                    IsMerged = targetCell.IsMerged,
                    ColSpan = targetCell.ColSpan,
                    RowSpan = targetCell.RowSpan,
                    Foreground = targetCell.Foreground,
                    Background = targetCell.Background,
                };
                draggedCellInfo.SetValue(draggedRow, theDraggedCell);
            }
            await Task.CompletedTask;
            return true;
        }

        internal TimetableCell GetTimetableCell(DayOfWeek day, int period) {
            TimetableRow draggedRow = TimetableRows.FirstOrDefault(x => x.Period == period);
            switch(day) {
                case DayOfWeek.Monday:
                    return draggedRow.Monday;
                case DayOfWeek.Tuesday:
                    return draggedRow.Tuesday;
                case DayOfWeek.Wednesday:
                    return draggedRow.Wednesday;
                case DayOfWeek.Thursday:
                    return draggedRow.Thursday;
                case DayOfWeek.Friday:
                    return draggedRow.Friday;
                case DayOfWeek.Saturday:
                    return draggedRow.Saturday;
                case DayOfWeek.Sunday:
                    return draggedRow.Sunday;
                default:
                    return null;
            }
        }
    }
}
