using Abp.Extensions;
using Abp.ObjectComparators.BooleanComparators;
using HandyControl.Controls;
using Serilog;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using XStudio.App.Helper;
using XStudio.App.Models.Data;
using XStudio.App.Models.Users;
using XStudio.App.Service;
using XStudio.App.ViewModel;
using XStudio.App.ViewModel.Module;
using XStudio.App.ViewModel.Users;

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
            PasswordBoxHelper.SetBindPassword(CodeTextBox, _loginViewModel.Password);
        }

        private void NameTextBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            if (_loginViewModel != null && sender is ComboBox box && box.SelectedItem is HistoryUserViewModel user) {
                _loginViewModel.UserNameOrEmailAddress = user.UserNameOrEmailAddress;
                _loginViewModel.Password = user.Password;
                _loginViewModel.RememberMe = user.RememberMe;
            }
        }

        /// <summary>
        /// 设置光标位置
        /// </summary>
        /// <param name="passwordBox"></param>
        /// <param name="start">光标开始位置</param>
        /// <param name="length">选中长度</param>
        private static void SetSelection(ComboBox comboBox, int start, int length) {
            comboBox.GetType()?
                    .GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic)?
                    .Invoke(comboBox, new object[] { start, length
            });
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e) {
            if (_loginViewModel != null && await OnLoginCompleted()) {
                LoginButton.IsChecked = false;
                Close();
            }
            else {
                LoginButton.IsChecked = false;
            }
        }

        private async Task<bool> OnLoginCompleted() {
            if (_loginViewModel == null) {
                return false;
            }
            try {
                UserViewModel user = await _loginViewModel.OnLoginAction();
                if (string.IsNullOrWhiteSpace(user?.TokenResponse?.AccessToken)) {
                    return false;
                }
                // 将User赋值给注册的单例
                ViewModelLocator.Instance.CurrentUser?.SetUser(user);
            }
            catch (Exception ex) {
                Growl.Clear(MessageToken.GrowlLoginWindow);
                Growl.Error(new HandyControl.Data.GrowlInfo() {
                     Message = ex.Message,
                     Token = MessageToken.GrowlLoginWindow,
                     WaitTime = 3,
                     FlowDirection= FlowDirection.LeftToRight,
                     Type = HandyControl.Data.InfoType.Error,
                     ShowDateTime = false,
                     Dispatcher = this.Dispatcher,
                     ShowCloseButton = true,
                     StaysOpen = false
                });
                Log.Error(ex, ex.Message);
                return false;
            }


            return true;
        }
    }
}
