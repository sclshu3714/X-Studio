using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.School.Timetable.ViewModels
{
    public class SettingsViewModel : MenuItemViewModel
    {
        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly HamburgerMenuControlViewModel hamburgerMenuViewModel;
        public SettingsViewModel(IDialogCoordinator dialogCoordinator, HamburgerMenuControlViewModel viewModel) 
            : base(viewModel)
        {
            _dialogCoordinator = dialogCoordinator;
            hamburgerMenuViewModel = viewModel;
        }
    }
}
