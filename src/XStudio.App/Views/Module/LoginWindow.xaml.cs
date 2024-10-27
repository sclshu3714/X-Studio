using Abp.ObjectComparators.BooleanComparators;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using XStudio.App.Service;
using XStudio.App.ViewModel.Module;

namespace XStudio.App.Views.Module {
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow {

        private LoginViewModel? _loginViewModel = null;

        public LoginWindow() {
            InitializeComponent();
        }

        internal void SetViewModel(LoginViewModel loginViewModel) {
            _loginViewModel = loginViewModel;
            DataContext = _loginViewModel;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();

        private void NameTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) {
            if (_loginViewModel == null) return;
            _loginViewModel.Password = "";
        }

        private void NameTextBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            if (_loginViewModel != null && sender is ComboBox box && box.SelectedItem is UserViewModel user) {
                _loginViewModel.UserNameOrEmailAddress = user.UserNameOrEmailAddress;
                _loginViewModel.Password = user.Password;
                _loginViewModel.RememberMe = user.RememberMe;
            }
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e) {
            // 禁用按钮以防止重复点击  
            LoginButton.IsEnabled = false;

            // 清除当前内容  
            LoginButton.Content = null;

            // 假设登录过程完成后有一个方法叫做 OnLoginCompleted  
            if (await OnLoginCompleted()) {
               Close();
            }
        }

        private async Task<bool> OnLoginCompleted() {
            // 登录验证
            await Task.Delay(10 * 1000);
            // 重置按钮内容  
            LoginButton.Content = "登 录";

            // 重新启用按钮  
            LoginButton.IsEnabled = true;
            await Task.CompletedTask;
            return true;
        }
    }
}
