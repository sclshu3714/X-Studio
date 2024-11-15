using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace XStudio.App.Models.Adorners {

    public class DataGridColumnDragAdorner : Adorner {
        private readonly DataGrid _dataGrid;
        private readonly DataGridColumn _draggedColumn;

        public DataGridColumnDragAdorner(DataGrid dataGrid, DataGridColumn draggedColumn)
            : base(dataGrid) {
            _dataGrid = dataGrid;
            _draggedColumn = draggedColumn;
            IsHitTestVisible = false;
        }

        protected override void OnRender(DrawingContext drawingContext) {
            base.OnRender(drawingContext);

            var header = GetColumnHeader(_draggedColumn);
            if (header != null) {
                header.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                header.Arrange(new Rect(header.DesiredSize));

                var renderTarget = new RenderTargetBitmap(
                    (int)header.ActualWidth,
                    (int)header.ActualHeight,
                    96d, 96d, PixelFormats.Default);

                renderTarget.Render(header); 
                drawingContext.DrawImage(renderTarget, new Rect(0,0, renderTarget.PixelWidth, renderTarget.PixelHeight));
            }
        }

        private DataGridColumnHeader GetColumnHeader(DataGridColumn column) {
            return (DataGridColumnHeader)column.Header;
        }
    }
}
