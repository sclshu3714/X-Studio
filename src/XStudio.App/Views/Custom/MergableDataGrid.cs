using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace XStudio.App.Views.Custom {
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
            var previousRowIndex = -1;

            for (int rowIndex = 0; rowIndex < Items.Count; rowIndex++) {
                for (int colIndex = 0; colIndex < Columns.Count; colIndex++) {
                    var currentItem = Items[rowIndex];
                    var currentValue = GetCellValue(rowIndex, colIndex);

                    // 获取当前单元格的边界
                    var cell = GetCell(rowIndex, colIndex);
                    if (cell != null) {
                        var bounds = new Rect(cell.TranslatePoint(new Point(0, 0), this), new Size(cell.ActualWidth, cell.ActualHeight));
                        if (currentValue?.ToString() == previousValue && previousRowIndex == rowIndex - 1) {
                            // 合并单元格
                            var previousCell = GetCell(previousRowIndex, colIndex);
                            if (previousCell != null) {
                                var previousBounds = new Rect(previousCell.TranslatePoint(new Point(0, 0), this), new Size(previousCell.ActualWidth, previousCell.ActualHeight));
                                bounds.Y = previousBounds.Y; // 设置合并后的单元格的 Y 坐标
                                bounds.Height += previousBounds.Height; // 增加高度
                                cellBounds[cellBounds.Count - 1] = bounds; // 更新合并单元格的边界
                            }
                        }
                        else {
                            // 记录当前单元格的边界
                            cellBounds.Add(bounds);
                        }

                        previousValue = currentValue?.ToString();
                        previousRowIndex = rowIndex;
                    }
                }
            }

            // 绘制合并的单元格
            foreach (var rect in cellBounds) {
                drawingContext?.DrawRectangle(Brushes.LightGray, null, rect); // 使用灰色背景绘制合并的单元格
            }
        }

        // 获取指定行和列的单元格的值
        public object? GetCellValue(int rowIndex, int columnIndex) {
            if (rowIndex < 0 || rowIndex >= Items.Count)
                throw new ArgumentOutOfRangeException(nameof(rowIndex));
            if (columnIndex < 0 || columnIndex >= Columns.Count)
                throw new ArgumentOutOfRangeException(nameof(columnIndex));

            // 获取指定行的项
            var item = Items[rowIndex];
            // 获取指定列的绑定路径
            var binding = (Columns[columnIndex] as DataGridBoundColumn)?.Binding as Binding;

            // 如果存在绑定路径，使用它来获取值
            if (binding != null) {
                var value = binding.Path.Path;
                var propertyInfo = item.GetType().GetProperty(value);
                return propertyInfo?.GetValue(item);
            }

            return null;
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
