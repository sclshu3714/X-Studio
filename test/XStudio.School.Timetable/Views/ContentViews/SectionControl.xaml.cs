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
        public SectionControl() {
            InitializeComponent();
        }

        private void TextBlock_DragOver(object sender, DragEventArgs e) {
            // 确保进行允许的拖放
            e.Effects = e.Data.GetDataPresent(typeof(TimetableCell)) ? DragDropEffects.Move : DragDropEffects.None;
        }

        private void TextBlock_Drop(object sender, DragEventArgs e) {
            if (sender is TextBlock targetTextBlock) {
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
            if (sender is TextBlock textBlock) {
                // 启动拖拽操作
                DragDrop.DoDragDrop(textBlock, textBlock.Tag, DragDropEffects.Move);
            }
        }

        private void CourseDataGrid_DragOver(object sender, DragEventArgs e) {
            e.Effects = e.Data.GetDataPresent(typeof(TimetableCell)) ? DragDropEffects.Move : DragDropEffects.None;
        }

        private void CourseDataGrid_Drop(object sender, DragEventArgs e) {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid == null) {
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

            // 交换数据
            if (targetCell != null && draggedCell != null && targetCell != draggedCell) {
                var targetProperty = targetCell.GetType().GetProperty("Content");
                var dragProperty = draggedCell.GetType().GetProperty("Content");

                if (targetProperty != null && dragProperty != null) {
                    var temptarget = targetProperty.GetValue(targetCell);
                    var tempdrag = dragProperty.GetValue(draggedCell);
                    targetProperty.SetValue(targetCell, tempdrag);
                    dragProperty.SetValue(draggedCell, temptarget);
                }
            }
        }

        private TimetableCell? GetDataGridItemAtMousePosition(DataGrid dataGrid, Point position) {
            var hit = dataGrid.InputHitTest(position) as FrameworkElement;
            if (hit is TextBlock textBlock) {
                return textBlock.Tag as TimetableCell;
            }
            else {
                while (hit != null && !(hit is DataGridCell)) {
                    hit = VisualTreeHelper.GetParent(hit) as FrameworkElement;
                }
                return hit?.DataContext as TimetableCell; // 替换为您的数据类型
            }
        }
    }
}
