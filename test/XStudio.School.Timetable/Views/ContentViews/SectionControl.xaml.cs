using ImTools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using XStudio.School.Timetable.Models;
using XStudio.SchoolSchedule;
using DayOfWeek = XStudio.SchoolSchedule.DayOfWeek;

namespace XStudio.School.Timetable.Views.ContentViews {
    /// <summary>
    /// Interaction logic for SectionControl
    /// </summary>
    public partial class SectionControl : UserControl {
        //public SectionControl() {
        //    InitializeComponent();
        //}

        //private void ScheduleGrid_LoadingRow(object sender, DataGridRowEventArgs e) {
        //    var row = e.Row;
        //    var item = row.Item as TimetableRow;
        //    if (item != null) {
        //        // 检查时段是否已经存在
        //        var existingCell = dataSection.Columns[0].GetCellContent(row);
        //        if (existingCell != null) {
        //            var existingRow = dataSection.ItemContainerGenerator.ContainerFromIndex(dataSection.Items.IndexOf(existingCell.DataContext)) as DataGridRow;
        //            if (existingRow != null) {
        //                // 设置行的CellStyle以合并单元格
        //                row.Style = new Style { BasedOn = row.Style };
        //                row.Style.Setters.Add(new Setter(DataGridRow.HeaderProperty, item.TimeSlot));
        //            }
        //        }
        //    }
        //}
        public ObservableCollection<Period> Periods { get; set; }

        public SectionControl() {
            InitializeComponent();

            Periods = new ObservableCollection<Period>
            {
                new Period { Name = "早晨", Start = TimeSpan.Parse("08:00:00"), End = TimeSpan.Parse("09:00:00"), Sections = new List<int> { 1, 2 } },
                new Period { Name = "上午", Start = TimeSpan.Parse("09:00:00"), End = TimeSpan.Parse("12:00:00"), Sections= new List<int> { 3, 4, 5, 6 } },
                new Period { Name = "中午", Start = TimeSpan.Parse("12:00:00"), End = TimeSpan.Parse("13:30:00"), Sections = new List<int> { 7 } },
                new Period { Name = "下午", Start = TimeSpan.Parse("13:30:00"), End = TimeSpan.Parse("17:00:00"), Sections = new List<int> { 8, 9, 10 } },
                new Period { Name = "晚上", Start = TimeSpan.Parse("17:00:00"), End = TimeSpan.Parse("21:00:00"), Sections = new List<int> { 11, 12, 13 } }
            };
            // LoadDataGrid();
        }

        private void LoadDataGrid() {
            // Create Period and Session columns  
            var periodColumn = new DataGridTextColumn { Header = "时段", Binding = new Binding("PeriodName") };
            var sessionColumn = new DataGridTextColumn { Header = "节次", Binding = new Binding("Session") };

            // Create dynamic day columns  
            var days = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };
            foreach (var day in days) {
                //var dayColumn = new DataGridTemplateColumn {
                //    Header = day.ToString(),
                //    CellTemplate = CreateDayCellTemplate(day),
                //    CellEditingTemplate = CreateDayCellEditingTemplate(day)
                //};
                CourseDataGrid.Columns.Add(new DataGridTextColumn { Header = $"{day.ToString()}" });
            }

            // Add Period and Session columns  
            CourseDataGrid.Columns.Insert(0, periodColumn);
            CourseDataGrid.Columns.Insert(1, sessionColumn);

            // Create and bind items source  
            var items = new ObservableCollection<ScheduleItem>();
            foreach (var period in Periods) {
                foreach (var section in period.Sections) {
                    var item = new ScheduleItem {
                        PeriodName = period.Name,
                        Session = section.ToString(),
                        Merged = false,
                        DaySchedules = new Dictionary<DayOfWeek, string>() { { DayOfWeek.Monday, GetCourseForPeriod(DayOfWeek.Monday, period, section) } }
                    };
                    items.Add(item);
                }
            }

            //// Merge cells for the same period  
            //var mergedGroups = items.GroupBy(i => i.PeriodName).ToList();
            //var mergedItems = new List<ScheduleItem>();
            //foreach (var group in mergedGroups) {
            //    var firstItem = group.First();
            //    firstItem.Merged = true;
            //    mergedItems.Add(firstItem);

            //    foreach (var day in days) {
            //        var courses = group.Where(it=>it.DaySchedules.ContainsKey(day)).Select(i => i.DaySchedules[day]).Distinct().ToList();
            //        if (courses.Count == group.Count()) {
            //            firstItem.DaySchedules[day] = string.Join(", ", courses);
            //        }
            //        else {
            //            firstItem.DaySchedules[day] = string.Empty; // or handle partially filled periods differently  
            //        }
            //    }
            //}

            CourseDataGrid.ItemsSource = items;
        }

        private string GetCourseForPeriod(DayOfWeek day, Period period, int session) {
            List<string> ClassCourseList = new() { "语文", "数学", "英语", "物理", "化学", "生物", "历史", "地理", "政治", "公共课", "体育", "美术", "音乐", "舞蹈", "戏剧", "电影", "健康课", "心理课", "综合课" };
            Random random = new(18);
            return ClassCourseList[random.Next(0, 18)];
            //if (daySchedule.Periods.TryGetValue(period, out var course) &&
            //    course.Contains($"{session}")) // Adjust this logic as needed  
            //{
            //    return course;
            //}
            //return string.Empty;
        }
        private DataTemplate CreatePeriodCellTemplate() {
            var factory = new FrameworkElementFactory(typeof(TextBlock));
            factory.SetBinding(TextBlock.TextProperty, new Binding("PeriodName"));
            factory.SetValue(TextBlock.StyleProperty, (Style)this.Resources["MergedCellStyle"]);
            return new DataTemplate { VisualTree = factory };
        }
        private DataTemplate CreateDayCellTemplate(DayOfWeek day) {
            var factory = new FrameworkElementFactory(typeof(TextBlock));
            factory.SetBinding(TextBlock.TextProperty, new MultiBinding {
                Converter = new DayScheduleConverter(day),
                Bindings =
                {
                    new Binding("DaySchedules"),
                    new Binding(nameof(ScheduleItem.Merged))
                }
            });
            factory.SetValue(TextBlock.StyleProperty, (Style)this.Resources["MergedCellStyle"]);
            return new DataTemplate { VisualTree = factory };
        }

        private DataTemplate CreateDayCellEditingTemplate(DayOfWeek day) {
            // For simplicity, we'll skip editing template in this example  
            return new DataTemplate();
        }

        private void CourseDataGrid_Drop(object sender, DragEventArgs e) {

            if (e.Data.GetDataPresent(typeof(TimetableRow))) // 替换为您的数据类型
            {
                var dataGrid = sender as DataGrid;
                var targetItem = GetDataGridItemAtMousePosition(dataGrid, e.GetPosition(dataGrid));

                if (targetItem != null) {
                    var draggedItem = e.Data.GetData(typeof(TimetableRow)) as TimetableRow; // 替换为您的数据类型

                    // 在这里交换数据
                    SwapItems(draggedItem, targetItem);
                }
            }
        }

        private void CourseDataGrid_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            var dataGrid = sender as DataGrid;
            var clickedItem = GetDataGridItemAtMousePosition(dataGrid, e.GetPosition(dataGrid));

            if (clickedItem != null) {
                DragDrop.DoDragDrop(dataGrid, clickedItem, DragDropEffects.Move);
            }
        }

        private TimetableRow GetDataGridItemAtMousePosition(DataGrid dataGrid, Point position) {
            var hit = dataGrid.InputHitTest(position) as FrameworkElement;
            while (hit != null && !(hit is DataGridRow)) {
                hit = VisualTreeHelper.GetParent(hit) as FrameworkElement;
            }
            return hit?.DataContext as TimetableRow; // 替换为您的数据类型
        }

        private void SwapItems(TimetableRow item1, TimetableRow item2) {
            // 在这里实现交换逻辑，例如更新视图模型中的集合
        }
    }

    public class Period {
        public string Name { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }

        public List<int> Sections { get; set; }
    }


    public class ScheduleItem {
        public string PeriodName { get; set; }
        public string Session { get; set; }
        public Dictionary<DayOfWeek, string> DaySchedules { get; set; }
        public bool Merged { get; set; }
    }

    public class DayScheduleConverter : IMultiValueConverter {
        private DayOfWeek _day;

        public DayScheduleConverter(DayOfWeek day) {
            _day = day;
        }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            var daySchedules = values[0] as Dictionary<DayOfWeek, string>;
            var merged = (bool)values[1];

            //if (daySchedules != null && daySchedules.TryGetValue(_day, out var course)) {
                return merged ? daySchedules.ElementAt(0).Value : new TextBlock { Text = daySchedules.ElementAt(0).Value, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center };
           // }
            //return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
