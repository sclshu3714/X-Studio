using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.App.Models
{
    public class DataModel
    {
        public int Index { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsSelected { get; set; }

        public string Remark { get; set; } = string.Empty;

        public DataType Type { get; set; }

        public string ImgPath { get; set; } = string.Empty;

        public ObservableCollection<DataModel> DataItems { get; set; } = [];
    }

}
