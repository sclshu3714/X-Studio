﻿using ImTools;
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
            Console.Write(JsonConvert.SerializeObject(classSchedule));
            List<Section> SectionList = classSchedule.Sections;
            for (int i = 1; i <= classSchedule.MaxPeriod; i++) {
                var sectionsForDay = new ObservableCollection<Section>(SectionList.Where(s => s.Period == i));
                Sections.Add(sectionsForDay);
            }
            ConvertToDataTable(classSchedule);
        }

        private void ConvertToDataTable(ClassSchedule classSchedule) {
            TimetableRows.Clear();
            List<Section> SectionList = classSchedule.Sections;
            for (int i = 1; i <= classSchedule.MaxPeriod; i++) {
                var sectionsForDay = new ObservableCollection<Section>(SectionList.Where(s => s.Period == i));
                if(!sectionsForDay.Any()) continue;
                TimetableRow timetable = new TimetableRow() {
                    ColSpan = 1,
                    RowSpan = 1,
                    TimeSlot = sectionsForDay.FirstOrDefault().TimePeriod,
                    Period = i,
                };
                foreach (var section in sectionsForDay) {
                    
                    switch (section.Day) { 
                        case DayOfWeek.Monday:
                            timetable.Day1 = section.Contents.FirstOrDefault()?.Content?.DisplayName;
                        break;
                        case DayOfWeek.Tuesday:
                            timetable.Day2 = section.Contents.FirstOrDefault()?.Content?.DisplayName;
                        break; 
                        case DayOfWeek.Wednesday:
                            timetable.Day3 = section.Contents.FirstOrDefault()?.Content?.DisplayName;
                        break;
                        case DayOfWeek.Thursday:
                            timetable.Day4 = section.Contents.FirstOrDefault()?.Content?.DisplayName;
                        break;
                        case DayOfWeek.Friday:
                            timetable.Day5 = section.Contents.FirstOrDefault()?.Content?.DisplayName;
                        break;
                        case DayOfWeek.Saturday:
                            timetable.Day6 = section.Contents.FirstOrDefault()?.Content?.DisplayName;
                        break;
                        case DayOfWeek.Sunday:
                            timetable.Day7 = section.Contents.FirstOrDefault()?.Content?.DisplayName;
                        break;
                    }
                    TimetableRows.Add(timetable);
                }
            }
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
            SetTimePeriods(classSchedule);
            // 设置通栏即合并单元格 - 不参与排课与自动排课
            classSchedule.SetColumnSpan(DayOfWeek.Monday, 5, 7); // 设置第5|8|9节从周1到周日合并单元格(通栏) - 不参与排课与自动排课
            classSchedule.SetColumnSpan(DayOfWeek.Monday, 8, 7);
            classSchedule.SetColumnSpan(DayOfWeek.Monday, 9, 7);
            // 排课 总课时: 16 * 7 = 112; 早读: 2 * 7 = 14 课时; 正课: 8 * 7 - 4 = 52 课时; 晚自习: 3 * 7 = 21 课时; 课间操: 1 * 7 = 7 课时; 午休: 2 * 7 = 14 课时;
            List<Tuple<string, float>> ClassCourseList = new() 
            {
                Tuple.Create("语文", 6.0f),
                Tuple.Create("数学", 6.0f),
                Tuple.Create("英语", 6.0f),
                Tuple.Create("物理", 4.5f),
                Tuple.Create("化学", 4.5f),
                Tuple.Create("生物", 4.5f),
                Tuple.Create("历史", 4.5f),
                Tuple.Create("地理", 4.5f),
                Tuple.Create("政治", 4.5f),
                Tuple.Create("体育", 1.0f),
                Tuple.Create("美术", 0.5f),
                Tuple.Create("音乐", 0.5f),
                Tuple.Create("班会", 1.0f),
                Tuple.Create("舞蹈", 0.1f),
                Tuple.Create("戏剧", 0.1f),
                Tuple.Create("电影", 0.1f),
                Tuple.Create("健康", 0.1f),
                Tuple.Create("心理", 0.1f),
                Tuple.Create("综合", 0.1f)
            };
            // 构建测试规则
            // 1 语文 6 课时
            // 2 数学 6 课时
            // 3 英语 6 课时 18 课时
            // 4 物理 4.5 课时
            // 5 化学 4.5 课时
            // 6 生物 4.5 课时
            // 7 历史 4.5 课时
            // 8 地理 4.5 课时
            // 9 政治 4.5 课时 4.5 * 6 = 27
            // 10 体育 1 课时
            // 11 美术 0.5 课时
            // 12 音乐 0.5 课时
            // 13 舞蹈、戏剧, 电影, 健康课, 心理课, 综合课 1 课时 3
            // 14 班会 1 课时
            Dictionary<string, ClassCourseRule> classCourses = ClassCourseList.ToDictionary(k => k.Item1, v => new ClassCourseRule() { Name = v.Item1, ClassHour = v.Item2, Mode = RuleMode.Course, Priority = PriorityMode.Medium, Type = RuleType.None });
            List<IRule> rules = new();
            // 语数英都有一个连堂
            rules.Add(new ConsecutiveClasses(PriorityMode.Highest, classCourses["语文"]) { ClassHour = 2 });
            rules.Add(new ConsecutiveClasses(PriorityMode.Highest, classCourses["数学"]) { ClassHour = 2 });
            rules.Add(new ConsecutiveClasses(PriorityMode.Highest, classCourses["英语"]) { ClassHour = 2 });
            rules.Add(new SingleOrBiweekly(PriorityMode.Highest, classCourses["物理"], classCourses["政治"]) { ClassHour = 1 });
            rules.Add(new SingleOrBiweekly(PriorityMode.Highest, classCourses["化学"], classCourses["生物"]) { ClassHour = 1 });
            rules.Add(new SingleOrBiweekly(PriorityMode.Highest, classCourses["历史"], classCourses["地理"]) { ClassHour = 1 });
            rules.Add(new SingleOrBiweekly(PriorityMode.Highest, classCourses["美术"], classCourses["音乐"]) { ClassHour = 1 });
            rules.Add(new AlternatePollingRule(PriorityMode.Highest, new List<ClassCourseRule>() { classCourses["舞蹈"], classCourses["戏剧"], classCourses["电影"], classCourses["健康"], classCourses["心理"], classCourses["综合"] }) { ClassHour = 1 });
            rules.Add(classCourses["语文"]);
            rules.Add(classCourses["语文"]);
            rules.Add(classCourses["语文"]);
            rules.Add(classCourses["语文"]);
            rules.Add(classCourses["数学"]);
            rules.Add(classCourses["数学"]);
            rules.Add(classCourses["数学"]);
            rules.Add(classCourses["数学"]); 
            rules.Add(classCourses["英语"]);
            rules.Add(classCourses["英语"]);
            rules.Add(classCourses["英语"]);
            rules.Add(classCourses["英语"]);
            rules.Add(classCourses["英语"]);
            rules.Add(classCourses["物理"]);
            rules.Add(classCourses["物理"]);
            rules.Add(classCourses["物理"]);
            rules.Add(classCourses["物理"]);
            rules.Add(classCourses["化学"]);
            rules.Add(classCourses["化学"]);
            rules.Add(classCourses["化学"]);
            rules.Add(classCourses["化学"]);
            rules.Add(classCourses["生物"]);
            rules.Add(classCourses["生物"]);
            rules.Add(classCourses["生物"]);
            rules.Add(classCourses["生物"]);
            rules.Add(classCourses["历史"]);
            rules.Add(classCourses["历史"]);
            rules.Add(classCourses["历史"]);
            rules.Add(classCourses["历史"]);
            rules.Add(classCourses["地理"]);
            rules.Add(classCourses["地理"]);
            rules.Add(classCourses["地理"]);
            rules.Add(classCourses["地理"]);
            rules.Add(classCourses["政治"]);
            rules.Add(classCourses["政治"]);
            rules.Add(classCourses["政治"]);
            rules.Add(classCourses["政治"]);
            rules.Add(classCourses["体育"]);
            AutoAssignCourses(classSchedule, rules, 0);
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

        // 设置时间段和节次的函数
        private void SetTimePeriods(ClassSchedule classSchedule) {
            classSchedule.SetSectionTimePeriod(new List<int>() { 1, 2 }, "早晨", SectionType.MorningStudy);
            classSchedule.SetSectionTimePeriod(new List<int>() { 3, 4 }, "上午", SectionType.RegularClass);
            classSchedule.SetSectionTimePeriod(new List<int>() { 5 }, "上午", SectionType.BreakExercise);
            classSchedule.SetSectionTimePeriod(new List<int>() { 6, 7 }, "上午", SectionType.RegularClass);
            classSchedule.SetSectionTimePeriod(new List<int>() { 8 }, "中午", SectionType.NoonBreak);
            classSchedule.SetSectionTimePeriod(new List<int>() { 9 }, "中午", SectionType.AfternoonStudy);
            classSchedule.SetSectionTimePeriod(new List<int>() { 10, 11, 12, 13 }, "下午", SectionType.RegularClass);
            classSchedule.SetSectionTimePeriod(new List<int>() { 14, 15, 16 }, "晚上", SectionType.EveningStudy);
        }

        // 自动分配课程的函数
        private bool AutoAssignCourses(ClassSchedule classSchedule, List<IRule> courses, int index) {
            if (index >= courses.Count) {
                return true; // 所有课程都成功分配
            }

            IRule rule = courses[index];

            // 遍历所有时段尝试分配
            var section = classSchedule.GetAvailableSections(rule, SectionType.RegularClass);
            if (classSchedule.CanAssign(rule, section)) {
                classSchedule.AddSectionContent(section.Code, new SectionContent(0, new ClassCourseRule() { Name = rule.DisplayName, Mode = rule.Mode }));

                // 递归尝试下一个课程
                if (AutoAssignCourses(classSchedule, courses, index + 1)) {
                    return true; // 找到了有效的课程安排
                }

                // 如果失败，撤销分配
                classSchedule.RemoveSectionContent(section.Code);
             }
            return false; // 该课程无法分配，返回失败
        }
    }
}