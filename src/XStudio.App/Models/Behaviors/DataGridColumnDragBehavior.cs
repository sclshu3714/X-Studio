using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using XStudio.App.Models.Adorners;
namespace XStudio.App.Models.Behaviors {
    public class DataGridColumnDragBehavior {
        public static readonly DependencyProperty EnableColumnDragProperty =
            DependencyProperty.RegisterAttached("EnableColumnDrag", typeof(bool), typeof(DataGridColumnDragBehavior), new PropertyMetadata(false, OnEnableColumnDragChanged));

        public static bool GetEnableColumnDrag(DependencyObject obj) {
            return (bool)obj.GetValue(EnableColumnDragProperty);
        }

        public static void SetEnableColumnDrag(DependencyObject obj, bool value) {
            obj.SetValue(EnableColumnDragProperty, value);
        }

        private static void OnEnableColumnDragChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (d is DataGrid dataGrid) {
                if ((bool)e.NewValue) {
                    dataGrid.LoadingRow += DataGrid_LoadingRow;
                }
                else {
                    dataGrid.LoadingRow -= DataGrid_LoadingRow;
                }
            }
        }

        private static void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e) {
            var dataGrid = sender as DataGrid;
            if (dataGrid != null) {
                foreach (var column in dataGrid.Columns) {
                    var header = GetColumnHeader(column);
                    if (header != null) {
                        header.PreviewMouseDown += Header_PreviewMouseDown;
                        header.PreviewMouseMove += Header_PreviewMouseMove;
                        header.MouseUp += Header_MouseUp;
                    }
                }
            }
        }

        private static DataGridColumnHeader GetColumnHeader(DataGridColumn column) {
            return (DataGridColumnHeader)column.Header;
        }

        private static DataGridColumn _draggedColumn;
        private static Point _dragStartPoint;

        private static void Header_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
            _draggedColumn = ((DataGridColumnHeader)sender).Column;
            _dragStartPoint = e.GetPosition(null);
        }

        private static void Header_PreviewMouseMove(object sender, MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed && _draggedColumn != null) {
                var pos = e.GetPosition(null);
                var diff = pos - _dragStartPoint;

                if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance) {
                    var dataGrid = (DataGrid)((DataGridColumnHeader)sender).Parent;
                    var dragAdornerLayer = AdornerLayer.GetAdornerLayer(dataGrid);

                    if (dragAdornerLayer != null) {
                        var adorner = new DataGridColumnDragAdorner(dataGrid, _draggedColumn);
                        dragAdornerLayer.Add(adorner);

                        dataGrid.MouseMove += DataGrid_MouseMoveForDrop;
                        dataGrid.MouseLeave += DataGrid_MouseLeaveForDrop;

                        _draggedColumn = null; // Reset to prevent multiple adorner instances
                    }
                }
            }
        }

        private static void Header_MouseUp(object sender, MouseButtonEventArgs e) {
            // Reset state
            _draggedColumn = null;
        }

        private static void DataGrid_MouseMoveForDrop(object sender, MouseEventArgs e) {
            var dataGrid = sender as DataGrid;
            var pos = e.GetPosition(dataGrid);

            var hitTestResults = VisualTreeHelper.HitTest(dataGrid, pos);
            if (hitTestResults != null && hitTestResults.VisualHit.GetType() == typeof(DataGridColumnHeader)) {
                var targetHeader = (DataGridColumnHeader)hitTestResults.VisualHit;
                var targetColumn = targetHeader.Column;

                if (targetColumn != null && targetColumn != _draggedColumnOriginal) {
                    int draggedIndex = dataGrid.Columns.IndexOf(_draggedColumnOriginal);
                    int targetIndex = dataGrid.Columns.IndexOf(targetColumn);

                    dataGrid.Columns.Remove(_draggedColumnOriginal);
                    dataGrid.Columns.Insert(targetIndex, _draggedColumnOriginal);

                    // Update _draggedColumnOriginal's reference in _draggedColumn (if needed)
                    // Since we removed and reinserted, _draggedColumnOriginal is still valid but just moved
                }
            }
        }

        private static DataGridColumn _draggedColumnOriginal;

        private static void DataGrid_MouseLeaveForDrop(object sender, MouseEventArgs e) {
            var dataGrid = sender as DataGrid;
            dataGrid.MouseMove -= DataGrid_MouseMoveForDrop;
            dataGrid.MouseLeave -= DataGrid_MouseLeaveForDrop;

            var adornerLayer = AdornerLayer.GetAdornerLayer(dataGrid);
            if (adornerLayer != null) {
                adornerLayer.Remove(adornerLayer.GetAdorners(dataGrid).OfType<DataGridColumnDragAdorner>().FirstOrDefault());
            }
        }
    }
}
