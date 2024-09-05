using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XStudio.App.Models;

namespace XStudio.App.ViewModel.Main
{
    public class WorkspaceInfoModel : ViewModelDataBase<WorkspaceItemModel>
    {
        public string Key { get; set; } = string.Empty;
        private string _title = string.Empty;
        private int _selectedIndex = -1;
        private int _index = 0;
        public WorkspaceInfoModel()
        {

        }

        public int Index
        {
            get => _index;
            set => SetProperty(ref _index, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetProperty(ref _selectedIndex, value);
        }

        public bool IsGroupEnabled { get; set; }
    }
}
