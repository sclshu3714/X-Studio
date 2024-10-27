using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Linq;

namespace XStudio.App.ViewModel.Module {
    public class UserViewModel : ViewModelBase {
        public int index = 0;
        public string userNameOrEmailAddress = string.Empty;
        public string password = string.Empty;
        public bool rememberMe = false;
        public bool isLoading = false;

        public int Index {
            get => index;
            set => SetProperty(ref index, value);
        }

        public string UserNameOrEmailAddress {
            get => userNameOrEmailAddress;
            set => SetProperty(ref userNameOrEmailAddress, value);
        }

        [StringLength(16, MinimumLength = 6, ErrorMessage = "密码长度 6-16")]
        public string Password {
            get => password;
            set => SetProperty(ref password, value);
        }
        public bool RememberMe {
            get => rememberMe;
            set => SetProperty(ref rememberMe, value);
        }

        public bool IsLoading {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }
    }
}
