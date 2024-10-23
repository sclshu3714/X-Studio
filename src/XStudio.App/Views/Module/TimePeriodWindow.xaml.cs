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
using XStudio.App.Models.Data;

namespace XStudio.App.Views.Module {
    /// <summary>
    /// TimePeriodWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TimePeriodWindow : Window {
        public TimePeriodWindow() {
            InitializeComponent();
            TimePeriodModel = new TimePeriod();
            TimePeriodModel.Order = Order;
            TimePeriodModel.Code = $"{Order}".PadLeft(6, '0');
            this.DataContext = TimePeriodModel;
        }

        public int Order { get; private set; } = 0;
        public TimePeriod TimePeriodModel { get; private set; }

        internal void SetOrder(int order) {
            Order = order;
            TimePeriodModel.Order = Order;
            TimePeriodModel.Code = $"{Order}".PadLeft(6, '0');
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = true;
            this.Close();
        }
    }
}
