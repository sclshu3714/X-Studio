using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XStudio.App.Models;

namespace XStudio.App.ViewModel.Main
{
    public class WorkspaceItemModel : ViewModelBase<DataModel>
    {
        private bool _isVisible = true;
        private string _queriesText = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string GroupName { get; set; } = string.Empty;

        public string TargetCtlName { get; set; } = string.Empty;

        public object? ImageBrush { get; set; } = null;

        public bool IsNew { get; set; } = false;

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
