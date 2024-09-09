using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using XStudio.App.Models;
using XStudio.App.ViewModel;

namespace XStudio.App.Models.Data
{
    public class WorkspaceItemModel : ViewModelDataBase<DataModel>
    {
        private bool _isVisible = true;
        private string _queriesText = string.Empty;
        private int _index = 0;
        private string _name = string.Empty;
        private string _groupName = string.Empty;
        private string _targetCtlName = string.Empty;
        private object? _imageBrush;
        private bool _isNew = true;

        public int Index
        {
            get => _index;
            set => SetProperty(ref _index, value);
        }


        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string GroupName
        {
            get => _groupName;
            set => SetProperty(ref _groupName, value);
        }

        public string TargetCtlName
        {
            get => _targetCtlName;
            set => SetProperty(ref _targetCtlName, value);
        }

        public object? ImageBrush
        {
            get => _imageBrush;
            set => SetProperty(ref _imageBrush, value);
        }

        public bool IsNew
        {
            get => _isNew;
            set => SetProperty(ref _isNew, value);
        }

        public string QueriesText
        {
            get => _queriesText;
            set => SetProperty(ref _queriesText, value);
        }

        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }
    }
}
