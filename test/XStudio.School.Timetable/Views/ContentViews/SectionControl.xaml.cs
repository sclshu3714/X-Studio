using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using XStudio.School.Timetable.Models;
using XStudio.School.Timetable.ViewModels;
using XStudio.SchoolSchedule;
using XStudio.SchoolSchedule.Enums;
using XStudio.SchoolSchedule.Rules;

namespace XStudio.School.Timetable.Views.ContentViews {
    /// <summary>
    /// Interaction logic for SectionControl
    /// </summary>
    public partial class SectionControl : UserControl {
        public SectionControl() {
            InitializeComponent();
        }

        private void TextBlock_DragOver(object sender, DragEventArgs e) {
            //if (e.Data.GetDataPresent(typeof(TimetableCell))) {
            //    e.Effects = DragDropEffects.Move;
            //}
            //else {
            //    e.Effects = DragDropEffects.None;
            //}
            e.Handled = true;
        }

        private void TextBlock_Drop(object sender, DragEventArgs e) {
            if (sender is TextBox targetTextBlock) {
                var targetDataContext = targetTextBlock.Tag as TimetableCell;
                var draggedDataContext = e.Data.GetData(typeof(TimetableCell));

                // 交换数据
                if (targetDataContext != null && draggedDataContext != null && targetDataContext != draggedDataContext) {
                    //// 假设 Day1 和 Day2 是数据模型的属性
                    //var targetProperty = targetDataContext.GetType().GetProperty("Day1");
                    //var dragProperty = draggedDataContext.GetType().GetProperty("Day1");

                    //if (targetProperty != null && dragProperty != null) {
                    //    var temp = targetProperty.GetValue(targetDataContext);
                    //    targetProperty.SetValue(targetDataContext, dragProperty.GetValue(draggedDataContext));
                    //    dragProperty.SetValue(draggedDataContext, temp);
                    //}
                }
            }
        }

        private void TextBlock_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (sender is TextBox textBox) {
                // 启动拖拽操作
                DragDrop.DoDragDrop(textBox, textBox.Tag, DragDropEffects.Move);
            }
        }
        private DragTitleAdorner theAdorner = null;
        private FrameworkElement dragelElement = null;
        private void CourseDataGrid_DragOver(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(typeof(TimetableCell)) && sender is DataGrid dataGrid) {
                e.Effects = DragDropEffects.Move;
                //显示装饰器
                System.Windows.Documents.AdornerLayer adornerLayer = System.Windows.Documents.AdornerLayer.GetAdornerLayer(sender as UIElement);
                if (adornerLayer != null) {
                    System.Windows.Documents.Adorner[] adorners = adornerLayer.GetAdorners(sender as UIElement);
                    if (adorners != null) {
                        foreach (var adorner in adorners) {
                            adornerLayer.Remove(adorner);
                        }
                    }
                    Point point = e.GetPosition(dataGrid);
                    if (dragelElement == null) {
                        dragelElement = GetFrameworkElementAtMousePosition(dataGrid, point);
                    }
                    theAdorner = new DragTitleAdorner(dataGrid, point, dragelElement);
                    adornerLayer.Add(theAdorner);
                }
            }
            else {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private async void CourseDataGrid_Drop(object sender, DragEventArgs e) {
            dragelElement = null;
            System.Windows.Documents.AdornerLayer adornerLayer = System.Windows.Documents.AdornerLayer.GetAdornerLayer(sender as UIElement);
            if (adornerLayer != null) {
                if (theAdorner != null) {
                    adornerLayer.Remove(theAdorner);
                    System.Windows.Documents.Adorner[] adorners = adornerLayer.GetAdorners(sender as UIElement);
                    if (adorners != null) {
                        foreach (var adorner in adorners) {
                            adornerLayer.Remove(adorner);
                        }
                    }
                }
                theAdorner = null;
            }
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid == null) {
                return;
            }
            SectionControlViewModel sectionControlViewModel = dataGrid.DataContext as SectionControlViewModel;
            if (sectionControlViewModel == null) {
                return;
            }
            var targetCell = GetDataGridItemAtMousePosition(dataGrid, e.GetPosition(dataGrid));
            if (targetCell == null) {
                // 放置位置没有数据
                return;
            }

            var draggedCell = e.Data.GetData(typeof(TimetableCell)) as TimetableCell;
            if (draggedCell == null) {
                return;
            }
            Section draggedSection = sectionControlViewModel.classSchedule[draggedCell.Day, draggedCell.Row.Period];
            Section targetSection = sectionControlViewModel.classSchedule[targetCell.Day, targetCell.Row.Period];
            if(draggedSection.Constraints.FirstOrDefault(x => x.Type == RuleType.CanOnlyArrange) is CanOnlyArrange theDCanOnlyArrange &&
               !targetSection.Contents.Any(x => theDCanOnlyArrange.Code.Contains(x.Content.Code))) {
                // 包含只能排课，不能交换
                return;
            }
            else if(targetSection.Constraints.FirstOrDefault(x => x.Type == RuleType.CanOnlyArrange) is CanOnlyArrange theTCanOnlyArrange &&
               !draggedSection.Contents.Any(x => theTCanOnlyArrange.Code.Contains(x.Content.Code))) {
                // 包含只能排课，不能交换
                return;
            }
            else if(draggedSection.Constraints.FirstOrDefault(x => x.Type == RuleType.CannotBeArranged) is CannotBeArranged theDCannotBeArranged &&
               targetSection.Contents.Any(x => theDCannotBeArranged.Code.Contains(x.Content.Code))) {
                // 包含只能排课，不能交换
                return;
            }
            else if(targetSection.Constraints.FirstOrDefault(x => x.Type == RuleType.CannotBeArranged) is CannotBeArranged theTCannotBeArranged &&
               draggedSection.Contents.Any(x => theTCannotBeArranged.Code.Contains(x.Content.Code))) {
                // 包含只能排课，不能交换
                return;
            }
            else if(targetSection.IsMergeCell || targetSection.LinkTo != null || 
                targetSection.Status != SectionStatus.Normal || targetSection.Type != draggedSection.Type) {
                return;
            }
            else if (targetCell.Day == draggedCell.Day && targetCell.Row.Period == draggedCell.Row.Period) {
                return;
            }
            if (targetSection.Contents.FirstOrDefault(x => x.Content.Type == RuleType.ConsecutiveClasses)?.Content is ConsecutiveClasses theConsecutiveClasses) {
                // 目标位置有连堂课/只能排时 不能交换
                return;
            }
            // 特殊处理连堂课
            if (draggedSection.Contents.FirstOrDefault(x => x.Content.Type == RuleType.ConsecutiveClasses)?.Content is ConsecutiveClasses lectureContent) {
                int index = lectureContent.Periods.IndexOf(draggedSection.Period);
                TimetableCell draggedCell1 = null;
                TimetableCell targetCell1 = null;
                Section draggedSection1 = null;
                Section targetSection1 = null;
                List<int> intPeriods = new List<int>();
                if (index == 0) { // 需要替换的节次 draggedSection.Period, draggedSection.Period + 1
                    draggedCell1 = sectionControlViewModel.GetTimetableCell(draggedCell.Day, draggedCell.Row.Period + 1);
                    targetCell1 = sectionControlViewModel.GetTimetableCell(targetCell.Day, targetCell.Row.Period + 1);
                    draggedSection1 = sectionControlViewModel.classSchedule[draggedCell.Day, draggedCell.Row.Period + 1];
                    targetSection1 = sectionControlViewModel.classSchedule[targetCell.Day, targetCell.Row.Period + 1];
                    intPeriods = new List<int> { targetCell.Row.Period, targetCell.Row.Period + 1 };
                }
                else if (index == 1) { // 需要替换的节次 draggedSection.Period - 1, draggedSection.Period
                    draggedCell1 = sectionControlViewModel.GetTimetableCell(draggedCell.Day, draggedCell.Row.Period - 1);
                    targetCell1 = sectionControlViewModel.GetTimetableCell(targetCell.Day, targetCell.Row.Period - 1);
                    draggedSection1 = sectionControlViewModel.classSchedule[draggedCell.Day, draggedCell.Row.Period - 1];
                    targetSection1 = sectionControlViewModel.classSchedule[targetCell.Day, targetCell.Row.Period - 1];
                    intPeriods = new List<int> { targetCell.Row.Period - 1, targetCell.Row.Period };
                }
                if (draggedSection1 == null || targetSection1 == null || targetSection1.IsMergeCell || 
                    targetSection1.LinkTo != null || targetSection1.Status != SectionStatus.Normal || 
                    targetSection1.Type != draggedSection1.Type) {
                    return;
                }

                Tuple<bool, string>  result = await sectionControlViewModel.classSchedule.ExchangeSessionsAsync(draggedSection.Code, targetSection.Code);
                Tuple<bool, string>  result1 = await sectionControlViewModel.classSchedule.ExchangeSessionsAsync(draggedSection1.Code, targetSection1.Code);
                if(result.Item1 && result1.Item1 &&
                    await sectionControlViewModel.ExchangeTimetableCellAsync(draggedCell, targetCell) &&
                    await sectionControlViewModel.ExchangeTimetableCellAsync(draggedCell1, targetCell1)) {
                    lectureContent.Periods = new List<int>(intPeriods);
                }
            }
            else {
                // 交换数据
                await sectionControlViewModel.ExchangeTimetableCellAsync(draggedCell, targetCell);
                Tuple<bool, string> tuple = await sectionControlViewModel.classSchedule.ExchangeSessionsAsync(draggedSection.Code, targetSection.Code);
            }
        }

        private TimetableCell GetDataGridItemAtMousePosition(DataGrid dataGrid, Point position) {
            var hit = dataGrid.InputHitTest(position) as FrameworkElement;
            if (hit is TextBox textBlock) {
                return textBlock.Tag as TimetableCell;
            }
            else {
                while (hit != null && !(hit is DataGridCell)) {
                    hit = VisualTreeHelper.GetParent(hit) as FrameworkElement;
                    if (hit is TextBox thehit) {
                        return thehit.Tag as TimetableCell;
                    }
                }
                TimetableRow row = hit?.DataContext as TimetableRow;
                if (row == null) {
                    return null;
                }
                int period = row.Period;
                int column = ((DataGridCell)hit).Column.DisplayIndex;
                DayOfWeek day = (DayOfWeek)(column - 1);
                TimetableCell theCell = row.GetType().GetProperty($"{day.ToString()}")?.GetValue(row) as TimetableCell;
                return theCell;
            }
        }

        private FrameworkElement GetFrameworkElementAtMousePosition(DataGrid dataGrid, Point position) {
            var hit = dataGrid.InputHitTest(position) as FrameworkElement;
            if (hit is TextBox textBlock) {
                return hit;
            }
            else {
                while (hit != null && !(hit is DataGridCell)) {
                    hit = VisualTreeHelper.GetParent(hit) as FrameworkElement;
                    if (hit is TextBox thehit) {
                        return hit;
                    }
                }
                if (hit == null) {
                    return null;
                }
                return hit;
            }
        }
    }
}
