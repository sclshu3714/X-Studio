using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using XStudio.School.Timetable.ViewModels;

namespace XStudio.School.Timetable.Views.DialogViews {
    /// <summary>
    /// Interaction logic for TimePeriodWindow.xaml
    /// </summary>
    public partial class TimePeriodWindow : MetroWindow {
        
        public TimePeriodWindowViewModel TimePeriodModel { get; private set; }
        public TimePeriodWindow(IDialogCoordinator _dialogCoordinator) {
            InitializeComponent();
            this.Icon = new BitmapImage(new Uri("pack://application:,,,/XStudio.School.Timetable;component/Assets/mylogo.jpg"));
            this.Loaded += (s, e) => {
                TimePeriodModel = new TimePeriodWindowViewModel(_dialogCoordinator);
                TimePeriodModel.TimePeriod.Order = Order;
                TimePeriodModel.TimePeriod.Code = $"{Order}".PadLeft(6, '0');
                this.DataContext = TimePeriodModel;
            };
        }

        public int Order { get; private set; } = 0;

        internal void SetOrder(int order) {
            Order = order;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = true;
            this.Close();
        }
    }
}
