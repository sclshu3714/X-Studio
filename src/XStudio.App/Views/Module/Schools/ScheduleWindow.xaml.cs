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
using System.Windows.Shapes;
using XStudio.App.ViewModel.Module.Schools;

namespace XStudio.App.Views.Module.Schools {
    /// <summary>
    /// ScheduleWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ScheduleWindow : Window {
        public ScheduleWindow() {
            InitializeComponent();
        }

        public ScheduleViewModel ScheduleModel { get; internal set; }

        internal void SetOrder(int v) {
            throw new NotImplementedException();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e) {

        }
    }
}
