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
        //protected override void OnRender(DrawingContext drawingContext) {
        //    this.drawingContext = drawingContext;
        //    base.OnRender(drawingContext);
        //    MergeCellsV2();
        //}


        protected override void OnItemsSourceChanged(System.Collections.IEnumerable oldValue, System.Collections.IEnumerable newValue) {
            base.OnItemsSourceChanged(oldValue, newValue);
            // 这里可以添加逻辑来处理数据源变化时的合并单元格
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
                        cell.Visibility = Visibility.Visible; // 隐藏合并的单元格
                    }
                    else {
                        // 记录当前单元格的边界
                        cellBounds.Add(bounds);
                    }

                    previousValue = currentValue?.ToString();
                    previousRowIndex = rowIndex;
                }
            }

            // 绘制合并的单元格
            foreach (var rect in cellBounds) {
                var rect1 = new Rect(new Point(0, 0), new Size(200, 200));
                drawingContext?.DrawRectangle(Brushes.Red, new Pen(Brushes.Red, 2), rect1); // 使用灰色背景绘制合并的单元格
            }
        }

        private void MergeCellsV2() {
            if (Items.Count == 0) return;

            string previousValue = string.Empty;
            int mergeStartRowIndex = -1;
            double mergeCellHeight = 0; // 保存合并单元格的高度

            for (int rowIndex = 0; rowIndex < Items.Count; rowIndex++) {
                TimetableRow currentRowItem = Items[rowIndex] as TimetableRow;
                string currentValue = currentRowItem.TimeSlot;

                DataGridCell cell = GetCell(rowIndex, 0); // 获取第0列的单元格
                if (cell == null) continue;

                if (currentValue == previousValue) {
                    // 如果当前值与前一个值相同
                    if (mergeStartRowIndex == -1) {
                        mergeStartRowIndex = rowIndex - 1; // 记录合并开始行

                        // 获取单元格高度用于后续计算
                        mergeCellHeight = cell.ActualHeight;
                    }
                    GetCell(mergeStartRowIndex, 0).Visibility = Visibility.Collapsed; // 隐藏合并开始行的单元格
                    // 隐藏当前单元格，避免重复显示
                    cell.Visibility = Visibility.Collapsed;
                }
                else {
                    // 当前值和前一个值不同，处理合并逻辑
                    if (mergeStartRowIndex != -1) {
                        // 计算合并后的高度
                        double mergedHeight = (rowIndex - mergeStartRowIndex) * mergeCellHeight;
                        Rect mergedBounds = new Rect(
                            GetCell(mergeStartRowIndex, 0).TranslatePoint(new Point(0, 0), this),
                            new Size(cell.ActualWidth, mergedHeight)
                        );

                        // 绘制合并单元格
                        drawingContext.DrawRectangle(Brushes.Red, new Pen(Brushes.Red, 2), mergedBounds);

                        // 重置合并状态
                        mergeStartRowIndex = -1;
                    }

                    // 更新前一个值
                    previousValue = currentValue;
                }

                // 更新当前行值
                previousValue = currentValue;
            }

            // 如果最后一组单元格仍在合并状态
            if (mergeStartRowIndex != -1) {
                double mergedHeight = (Items.Count - mergeStartRowIndex) * mergeCellHeight;
                Rect mergedBounds = new Rect(
                    GetCell(mergeStartRowIndex, 0).TranslatePoint(new Point(0, 0), this),
                    new Size(GetCell(mergeStartRowIndex, 0).ActualWidth, mergedHeight)
                );
                mergedBounds = new Rect(0,0,300,300);
                drawingContext.DrawRectangle(Brushes.Red, new Pen(Brushes.Red, 2), mergedBounds);
                mergeStartRowIndex = -1; // 重置合并状态
            }
        }

        private object GetCellContent(int row, int col) {
            // 根据行和列索引获取单元格内容
            TimetableRow currentRowItem = Items[row] as TimetableRow;
            TimetableCell currentCellItem = currentRowItem[col]; // 时段列
            return currentCellItem;
        }

        private Rect GetCellBounds(int row, int col) {
            // 计算单元格的边界
            var cellWidth = ActualWidth / Columns.Count;
            var cellHeight = ActualHeight / Items.Count;
            return new Rect(col * cellWidth, row * cellHeight, cellWidth, cellHeight);
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
