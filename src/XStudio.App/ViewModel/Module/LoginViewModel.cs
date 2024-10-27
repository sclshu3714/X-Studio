using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using XStudio.App.Models.Data;
using XStudio.App.Models.Users;
using XStudio.App.Service;
using XStudio.App.ViewModel.Main;
using XStudio.App.Views.Module;

namespace XStudio.App.ViewModel.Module {
    public class LoginViewModel : ViewModelBase {
        private DataService _dataService;
        public LoginViewModel(DataService dataService) {
            //MessengerInstance = WeakReferenceMessenger.Default;
            _dataService = dataService;
            LoginCommand = new DelegateCommand<LoginViewModel>(OnLoginAction);
        }

        /// <summary>
        /// 记录5个历史登录账号
        /// </summary>
        public ObservableCollection<UserViewModel> DefaultSelectList {
            get => defaultSelectList;
            set => SetProperty(ref defaultSelectList, value);
        }

        private ObservableCollection<UserViewModel> defaultSelectList = new ObservableCollection<UserViewModel>() { 
            new UserViewModel(){ index = 0, userNameOrEmailAddress="admin", password="1q2w3E*"  },
        };

        public string Password { 
            get => _password;
            set => SetProperty(ref _password, value);
        }
        private string _password = string.Empty;

        public bool? RememberMe {
            get => _rememberMe;
            set => SetProperty(ref _rememberMe, value);
        }
        private bool? _rememberMe = false;
        public string UserNameOrEmailAddress { 
            get => _userNameOrEmailAddress;
            set => SetProperty(ref _userNameOrEmailAddress, value);
        }
        private string _userNameOrEmailAddress = string.Empty;

        public DelegateCommand<LoginViewModel> LoginCommand { get; }

        private void OnLoginAction(LoginViewModel model) {
            //MessengerInstance?.Send<object, string>(view, MessageToken.LoadShowContent);
        }
    }
}
