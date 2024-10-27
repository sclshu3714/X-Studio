using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using XStudio.App.Helper;
using XStudio.App.Models.Data;

namespace XStudio.App.ViewModel.Main
{
    public class NonClientAreaViewModel : ViewModelBase
    {
        public NonClientAreaViewModel() 
        {
            //MessengerInstance = WeakReferenceMessenger.Default;
            VersionInfo = VersionHelper.GetVersion();
        }
        public RelayCommand<string> OpenViewCmd => new(OpenView);
        public RelayCommand<object> LoginViewCmd => new(LoginView);

        private void LoginView(object? obj) {
            if (obj == null) return;
            MessengerInstance?.Send<object, string>(MessageToken.LoginWindow, MessageToken.LoginWindow);
        }

        private void OpenView(string? viewName)
        {
            MessengerInstance?.Send<object, string>("", MessageToken.ClearLeftSelected);
            MessengerInstance?.Send<object, string>(true, MessageToken.FullSwitch);
            object? view = AssemblyHelper.CreateInternalInstance($"UserControl.{viewName}");
            if (view is not null) {
                MessengerInstance?.Send<object, string>(view, MessageToken.LoadShowContent);
            }
        }

        private string _versionInfo = string.Empty;

        public string VersionInfo
        {
            get => _versionInfo;
            set => Set(ref _versionInfo, value);
        }
    }
}
