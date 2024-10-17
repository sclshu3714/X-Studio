﻿using MahApps.Metro.Controls.Dialogs;
using Prism.Mvvm;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace XStudio.School.Timetable.ViewModels {
    public class MainWindowViewModel : BindableBase {
        private string _title = "Prism Application";
        
        public MainWindowViewModel() {
            
        }

        public string Title {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
    }
}
