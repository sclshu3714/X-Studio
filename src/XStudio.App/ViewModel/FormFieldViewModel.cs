using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using XStudio.App.Models.Enums;

namespace XStudio.App.ViewModel
{
    public class FormFieldViewModel : ViewModelBase
    {
        private string _label = string.Empty;
        private string _value = string.Empty;
        private FormFieldType _fieldType = FormFieldType.Text;
        private ObservableCollection<string> _defaultSelect = new ObservableCollection<string>();

        public string Label {
            get => _label; 
            set => SetProperty(ref _label, value);
        }

        public string Value {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        public FormFieldType FieldType {
            get => _fieldType;
            set => SetProperty(ref _fieldType, value);
        }

        /// <summary>
        /// 默认可选项
        /// </summary>
        public ObservableCollection<string> DefaultSelect {
            get => _defaultSelect;
            set => SetProperty(ref _defaultSelect, value);
        }
    }
}
