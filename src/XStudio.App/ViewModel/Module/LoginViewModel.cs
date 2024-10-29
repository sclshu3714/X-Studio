using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using XStudio.App.Models.Data;
using XStudio.App.Models.Users;
using XStudio.App.Service;
using XStudio.App.ViewModel.Main;
using XStudio.App.ViewModel.Users;
using XStudio.App.Views.Module;

namespace XStudio.App.ViewModel.Module {
    public class LoginViewModel : ViewModelBase, IDataErrorInfo {
        private DataService _dataService;
        public LoginViewModel(DataService dataService) {
            //MessengerInstance = WeakReferenceMessenger.Default;
            _dataService = dataService;
            LoginCommand = new DelegateCommand<LoginViewModel>(OnLoginAction);
        }

        /// <summary>
        /// 记录5个历史登录账号
        /// </summary>
        public ObservableCollection<HistoryUserViewModel> DefaultSelectList {
            get => defaultSelectList;
            set => SetProperty(ref defaultSelectList, value);
        }

        private ObservableCollection<HistoryUserViewModel> defaultSelectList = new ObservableCollection<HistoryUserViewModel>() { 
            new HistoryUserViewModel(){ index = 0, userNameOrEmailAddress="admin", password="1q2w3E*"  },
        };

        public string Password { 
            get => _password;
            set => SetProperty(ref _password, value);
        }
        private string _password = string.Empty;

        public bool RememberMe {
            get => _rememberMe;
            set => SetProperty(ref _rememberMe, value);
        }
        private bool _rememberMe = false;
        public string UserNameOrEmailAddress { 
            get => _userNameOrEmailAddress;
            set => SetProperty(ref _userNameOrEmailAddress, value);
        }
        private string _userNameOrEmailAddress = string.Empty;

        public DelegateCommand<LoginViewModel> LoginCommand { get; }

        public string Error => string.Empty;

        public string this[string columnName] {
            get {
                var vc = new ValidationContext(this, null, null);
                vc.MemberName = columnName;
                var res = new List<ValidationResult>();
                var result = Validator.TryValidateProperty(this.GetType().GetProperty(columnName)?.GetValue(this, null), vc, res);
                if (res.Count > 0) {
                    return string.Join(Environment.NewLine, res.Select(r => r.ErrorMessage).ToArray());
                }
                return string.Empty;
            }
        }

        public void OnLoginAction(LoginViewModel model) {
            //MessengerInstance?.Send<object, string>(view, MessageToken.LoadShowContent);
        }

        public async Task<UserViewModel?> OnLoginAction() {
            UserViewModel? user = await _dataService.LoginAsync(UserNameOrEmailAddress, Password, RememberMe);
            return user;
        }

        public HistoryUserViewModel? GetUserViewModel(string userNameOrEmailAddress) {
            if (string.IsNullOrEmpty(userNameOrEmailAddress)) {
                return null;
            }
            return DefaultSelectList.FirstOrDefault(u => u.UserNameOrEmailAddress == userNameOrEmailAddress);
        }
    }
}
