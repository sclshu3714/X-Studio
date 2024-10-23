using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace XStudio.School.Timetable.Models {
    //public class DragThumbnailEffect : DefaultDragEffect {
    //    protected override void InitializeDragDropEffects() {
    //        // 创建缩略图
    //        var thumbnail = new Image();
    //        thumbnail.Width = 100;
    //        thumbnail.Height = 100;
    //        thumbnail.Source = new BitmapImage(new Uri("你的缩略图路径", UriKind.RelativeOrAbsolute));
    //        thumbnail.SetValue(Canvas.LeftProperty, Mouse.GetPosition(this.AssociatedObject).X - thumbnail.Width / 2);
    //        thumbnail.SetValue(Canvas.TopProperty, Mouse.GetPosition(this.AssociatedObject).Y - thumbnail.Height / 2);

    //        // 添加到AdornerLayer
    //        var adornerLayer = AdornerLayer.GetAdornerLayer(this.AssociatedObject);
    //        adornerLayer.Add(new ThumbnailAdorner(this.AssociatedObject, thumbnail));
    //    }
    //}

    public class ThumbnailAdorner : Adorner {
        private VisualCollection visuals;
        private UIElement thumbnail;

        public ThumbnailAdorner(UIElement adornedElement, UIElement thumbnail)
            : base(adornedElement) {
            this.thumbnail = thumbnail;
            visuals = new VisualCollection(this);
            visuals.Add(this.thumbnail);
        }

        protected override Size ArrangeOverride(Size finalSize) {
            this.thumbnail.Arrange(new Rect(this.thumbnail.DesiredSize));
            return base.ArrangeOverride(finalSize);
        }

        protected override Visual GetVisualChild(int index) {
            return visuals[index];
        }

        protected override int VisualChildrenCount => visuals.Count;
    }

    public class DragTitleAdorner : Adorner {
        private readonly ContentPresenter _contentPresenter;
        private Control Control {
            get {
                return (Control)this.AdornedElement;
            }
        }

        public DragTitleAdorner(UIElement adornedElement, Point pos, string? Title = "") : base(adornedElement) {
            IsHitTestVisible = false;

            int width = 22;
            if (Title != null) {
                width += (int)MeasureTextWidth(Title, 14, "宋体");
            }

            this._contentPresenter = new ContentPresenter {
                Content = new Border {
                    Background = Brushes.SteelBlue,
                    Width = width,
                    Height = 28,
                    BorderBrush = Brushes.Gray,
                    BorderThickness = new Thickness(1),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    CornerRadius = new CornerRadius(5),
                    Child = new TextBlock {
                        Text = Title,
                        FontSize = 14,
                        FontFamily = new FontFamily("宋体"),
                        Foreground = Brushes.White,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(10, 0, 0, 0),
                    },
                },
            };

            double left = pos.X;
            double top = pos.Y;
            this.Margin = new Thickness(left + 5, top + 10, 0, 0);
        }

        public DragTitleAdorner(UIElement adornedElement, Point pos, FrameworkElement element) : base(adornedElement) {
            IsHitTestVisible = false;

            double width = 22;
            double height = 3;
            Image image = null;
            if (element != null) {
                image = CreateThumbnail(element);
                image.Height = element.ActualHeight;
                image.Width = element.ActualWidth;
                width += element.ActualWidth;
                height += element.ActualHeight;
            }

            this._contentPresenter = new ContentPresenter {
                Content = new Border {
                    Background = Brushes.SteelBlue,
                    Width = width,
                    Height = height,
                    BorderBrush = Brushes.Gray,
                    BorderThickness = new Thickness(1),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    CornerRadius = new CornerRadius(5),
                    Child = image,
                },
            };

            double left = pos.X;
            double top = pos.Y;
            this.Margin = new Thickness(left + 5, top + 10, 0, 0);
        }

        public void UpdatePosition(Point pos) {
            double left = pos.X + 5;
            double top = pos.Y + 10;

            // 获取父容器的尺寸
            double parentWidth = ((FrameworkElement)this.Parent).ActualWidth;
            double parentHeight = ((FrameworkElement)this.Parent).ActualHeight;

            // 确保控件不会超出右边界
            if (left + this.ActualWidth > parentWidth) {
                left = parentWidth - this.ActualWidth - 5; // 适当位置偏移
            }

            // 确保控件不会超出下边界
            if (top + this.ActualHeight > parentHeight) {
                top = parentHeight - this.ActualHeight - 10; // 适当位置偏移
            }

            this.Margin = new Thickness(left, top, 0, 0);
        }

        #region Override

        protected override int VisualChildrenCount {
            get {
                return 1;
            }
        }

        protected override Visual GetVisualChild(int index) // replace the Visual of the TextBox with the visual of the _contentPresenter;
        {
            return this._contentPresenter;
        }

        protected override Size MeasureOverride(Size constraint) {
            this._contentPresenter.Measure(this.Control.RenderSize); // delegate the measure override to the ContentPresenter's Measure
            return this.Control.RenderSize;
        }

        protected override Size ArrangeOverride(Size finalSize) {
            this._contentPresenter.Arrange(new Rect(finalSize));
            return finalSize;
        }

        #endregion Override

        private double MeasureTextWidth(string text, double fontSize, string fontFamily) {
            FormattedText formattedText = new FormattedText(
            text,
            System.Globalization.CultureInfo.InvariantCulture,
            FlowDirection.LeftToRight,
            new Typeface(fontFamily.ToString()),
            fontSize,
            Brushes.Black,
            96
            );
            return formattedText.WidthIncludingTrailingWhitespace;
        }

        public Image CreateThumbnail(FrameworkElement uIElement) {
            // 创建 RenderTargetBitmap 实例
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
                (int)uIElement.ActualWidth,
                (int)uIElement.ActualHeight,
                96d,
                96d,
                System.Windows.Media.PixelFormats.Pbgra32);

            // 将控件渲染到 RenderTargetBitmap
            renderBitmap.Render(uIElement);

            // 将 RenderTargetBitmap 转换为 BitmapImage
            BitmapImage bitmapImage = new BitmapImage();
            using (var memoryStream = new System.IO.MemoryStream()) {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder.Save(memoryStream);
                memoryStream.Position = 0;

                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); // 使 BitmapImage 可在不同线程中使用
            }
            var ThumbnailImage = new Image();
            // 将 BitmapImage 设置为 Image 控件的 Source
            ThumbnailImage.Source = bitmapImage;
            return ThumbnailImage;
        }
    }
}
