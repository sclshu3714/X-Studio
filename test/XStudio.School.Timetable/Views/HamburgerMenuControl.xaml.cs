using System.Windows.Controls;

namespace XStudio.School.Timetable.Views {
    /// <summary>
    /// Interaction logic for HamburgerMenuControl
    /// </summary>
    public partial class HamburgerMenuControl : UserControl {
        public HamburgerMenuControl() {
            InitializeComponent();
        }

        private void HamburgerMenuControl_OnItemInvoked(object sender, MahApps.Metro.Controls.HamburgerMenuItemInvokedEventArgs args) {
            this.theHamburgerMenuControl.Content = args.InvokedItem;
        }
    }
}
