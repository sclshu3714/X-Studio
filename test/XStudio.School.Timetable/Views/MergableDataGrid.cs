using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;
using XStudio.School.Timetable.Models;

namespace XStudio.School.Timetable.Views {
    public class MergableDataGrid : DataGrid {
        private DrawingContext? drawingContext = null;
        protected override void OnRender(DrawingContext drawingContext) {
            this.drawingContext = drawingContext;
            base.OnRender(drawingContext);
            MergeCells();
        }

        private void MergeCells() {
            if (Items.Count == 0) return;

            var cellBounds = new List<Rect>();
            var previousValue = string.Empty;
            var currentValue = string.Empty;
            var previousRowIndex = -1;

            for (int rowIndex = 0; rowIndex < Items.Count; rowIndex++) {
                TimetableRow currentRowItem = Items[rowIndex] as TimetableRow;
                currentValue = currentRowItem.TimeSlot;
                TimetableCell currentCellItem = currentRowItem[0]; // 时段列
                var cell = GetCell(rowIndex, 0);
                if (cell != null) {
                    var bounds = new Rect(cell.TranslatePoint(new Point(0, 0), this), new Size(cell.ActualWidth, cell.ActualHeight));
                    if (currentValue?.ToString() == previousValue && previousRowIndex == rowIndex - 1 && rowIndex > 0) {
                        // 合并单元格
                        //var previousCell = GetCell(previousRowIndex, 0);
                        //if (previousCell != null) {
                        //    var previousBounds = new Rect(previousCell.TranslatePoint(new Point(0, 0), this), new Size(previousCell.ActualWidth, previousCell.ActualHeight));
                        //    bounds.Y = previousBounds.Y; // 设置合并后的单元格的 Y 坐标
                        //    bounds.Height += previousBounds.Height; // 增加高度
                        //    cellBounds[cellBounds.Count - 1] = bounds; // 更新合并单元格的边界
                        //}
                        cell.Visibility = Visibility.Collapsed; // 隐藏合并的单元格
                    }
                    else {
                        // 记录当前单元格的边界
                        cellBounds.Add(bounds);
                    }

                    previousValue = currentValue?.ToString();
                    previousRowIndex = rowIndex;
                }
            }

            //// 绘制合并的单元格
            //foreach (var rect in cellBounds) {
            //    var rect1 = new Rect(new Point(0, 0), new Size(200, 200));
            //    drawingContext?.DrawRectangle(Brushes.Red, new Pen(Brushes.Red, 2), rect1); // 使用灰色背景绘制合并的单元格
            //}
        }

        private DataGridCell? GetCell(int rowIndex, int columnIndex) {
            var row = ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            if (row != null) {
                var cellPresenter = GetVisualChild<DataGridCellsPresenter>(row);
                if (cellPresenter != null) {
                    return (DataGridCell)cellPresenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
                }
            }
            return null;
        }

        private T? GetVisualChild<T>(DependencyObject parent) where T : DependencyObject {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++) {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild) {
                    return typedChild;
                }

                var childOfChild = GetVisualChild<T>(child);
                if (childOfChild != null) {
                    return childOfChild;
                }
            }
            return null;
        }
    }
}
