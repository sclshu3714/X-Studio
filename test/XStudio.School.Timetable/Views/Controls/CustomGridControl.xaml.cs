using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace XStudio.School.Timetable.Views.Controls {
    /// <summary>
    /// CustomGridControl.xaml 的交互逻辑
    /// </summary>
    public partial class CustomGridControl : UserControl {
        private object _draggedData;
        public CustomGridControl() {
            InitializeComponent();
        }

        private void Cell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            var border = (Border)sender;
            _draggedData = border.DataContext;
            DragDrop.DoDragDrop(border, border.DataContext, DragDropEffects.Move);
        }

        private void Cell_Drop(object sender, DragEventArgs e) {
            //var targetBorder = (Border)sender;
            //var targetData = targetBorder.DataContext;
            //var sourceData = e.Data.GetData(typeof(CellViewModel));

            //// 交换数据
            //var cells = (ObservableCollection<CellViewModel>)ItemsControl.ItemsSource;
            //int sourceIndex = cells.IndexOf((CellViewModel)sourceData);
            //int targetIndex = cells.IndexOf((CellViewModel)targetData);

            //if(sourceIndex != -1 && targetIndex != -1) {
            //    cells.Move(sourceIndex, targetIndex);
            //}
        }

        private void Border_DragEnter(object sender, DragEventArgs e) {

        }
    }
}
