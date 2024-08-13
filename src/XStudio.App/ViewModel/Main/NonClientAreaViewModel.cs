using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XStudio.App.Helper;
using XStudio.App.Models.Data;

namespace XStudio.App.ViewModel.Main
{
    public class NonClientAreaViewModel : ViewModelBase
    {
        public NonClientAreaViewModel()
        {
            VersionInfo = VersionHelper.GetVersion();
        }
        public RelayCommand<string> OpenViewCmd => new(OpenView);

        private void OpenView(string? viewName)
        {
            MessengerInstance?.Send<object, string>("", MessageToken.ClearLeftSelected);
            MessengerInstance?.Send<object, string>(true, MessageToken.FullSwitch);
            MessengerInstance?.Send<object, string>(AssemblyHelper.CreateInternalInstance($"UserControl.{viewName}"), MessageToken.LoadShowContent);
        }

        private string _versionInfo = string.Empty;

        public string VersionInfo
        {
            get => _versionInfo;
            set => Set(ref _versionInfo, value);
        }
    }
}
