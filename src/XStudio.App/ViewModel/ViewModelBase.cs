using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace XStudio.App.ViewModel
{
    public class ViewModelBase<T> : ObservableObject
    {
        private IList<T> _dataList = [];

        public IList<T> DataList
        {
            get => _dataList;
            set => SetProperty(ref _dataList, value);
        }
    }

}
