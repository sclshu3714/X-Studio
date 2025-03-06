using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XStudio.School.Timetable.Utilities {
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:XStudio.School.Timetable.Utilities"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:XStudio.School.Timetable.Utilities;assembly=XStudio.School.Timetable.Utilities"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:CustomGridPanel/>
    ///
    /// </summary>
    public class CustomGridPanel : Panel {
        static CustomGridPanel() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomGridPanel), new FrameworkPropertyMetadata(typeof(CustomGridPanel)));
        }
        public CustomGridPanel() {
        }
        protected override Size MeasureOverride(Size availableSize) {
            foreach(UIElement child in Children) {
                child.Measure(availableSize);
            }
            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize) {
            // 动态计算行列布局
            int columns = 5; // 示例列数
            int rows = Children.Count / columns;

            double cellWidth = finalSize.Width / columns;
            double cellHeight = finalSize.Height / rows;

            for(int i = 0; i < Children.Count; i++) {
                var child = Children[i];
                int row = i / columns;
                int col = i % columns;

                // 应用合并跨度
                int colSpan = GridCellProperties.GetColumnSpan(child);
                int rowSpan = GridCellProperties.GetRowSpan(child);

                child.Arrange(new Rect(
                    col * cellWidth,
                    row * cellHeight,
                    cellWidth * colSpan,
                    cellHeight * rowSpan));
            }
            return finalSize;
        }
    }
}
