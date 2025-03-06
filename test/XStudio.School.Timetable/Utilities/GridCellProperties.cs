using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XStudio.School.Timetable.Utilities {
    // 合并单元格附加属性
    public static class GridCellProperties {
        public static readonly DependencyProperty ColumnSpanProperty =
            DependencyProperty.RegisterAttached("ColumnSpan", typeof(int), typeof(GridCellProperties),
                new PropertyMetadata(1));

        public static readonly DependencyProperty RowSpanProperty =
            DependencyProperty.RegisterAttached("RowSpan", typeof(int), typeof(GridCellProperties),
                new PropertyMetadata(1));

        public static int GetColumnSpan(DependencyObject obj) => (int)obj.GetValue(ColumnSpanProperty);
        public static void SetColumnSpan(DependencyObject obj, int value) => obj.SetValue(ColumnSpanProperty, value);

        public static int GetRowSpan(DependencyObject obj) => (int)obj.GetValue(RowSpanProperty);
        public static void SetRowSpan(DependencyObject obj, int value) => obj.SetValue(RowSpanProperty, value);
    }
}
