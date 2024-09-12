using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using XStudio.App.Service;

namespace XStudio.App.ViewModel.Home
{
    public class HomePageViewModel : ViewModelDataBase<Page>
    {
        private readonly DataService _dataService;
        public HomePageViewModel(DataService dataService) {
            _dataService = dataService;
            DataList = dataService.HomePages();
        }
    }
}
