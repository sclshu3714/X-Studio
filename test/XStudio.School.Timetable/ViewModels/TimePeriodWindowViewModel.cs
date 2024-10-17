using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using XStudio.School.Timetable.Models;

namespace XStudio.School.Timetable.ViewModels {
    public class TimePeriodWindowViewModel : BindableBase {
        private readonly IDialogCoordinator _dialogCoordinator;
        private TimePeriod _timePeriod;
        public TimePeriodWindowViewModel(IDialogCoordinator dialogCoordinator) {
            _dialogCoordinator = dialogCoordinator;
            TimePeriod = new TimePeriod();
        }
        
        public TimePeriod TimePeriod {
            get { return _timePeriod; }
            set { SetProperty(ref _timePeriod, value); }
        }

        public ICommand ControlButtonCommand => new DelegateCommand<object>(async (parameter) => {
            
        });
    }
}
