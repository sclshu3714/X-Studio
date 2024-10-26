using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using XStudio.App.Models.Data;
using XStudio.App.ViewModel.Main;
using XStudio.App.Views.Module;

namespace XStudio.App.ViewModel.Module {
    public class LoginViewModel : ViewModelBase {
        public LoginViewModel() {
            LoginCommand = new DelegateCommand<LoginViewModel>(OnLoginAction);
            // 订阅消息
            //Messenger.Default.Register<object>(this, MessageToken.LoginWindow, OnLoginWindow);
        }

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
            throw new NotImplementedException();
        }
    }
}
