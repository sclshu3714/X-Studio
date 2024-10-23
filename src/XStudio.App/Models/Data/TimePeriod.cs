using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XStudio.App.ViewModel;

namespace XStudio.App.Models.Data {
    public class TimePeriod : ViewModelBase {
        private int _order = 0;
        private string _code = string.Empty;
        private string _name = string.Empty;

        private List<string> defaultSelectList = new List<string>() { "早晨", "上午", "中午", "下午", "晚上" };
        /// <summary>
        /// 序号
        /// </summary>
        [Description("序号")]
        public int Order {
            get => _order;
            set => SetProperty(ref _order, value);
        }

        /// <summary>
        /// 时段编号
        /// </summary>
        [Description("编码")]
        public string Code {
            get => _code;
            set => SetProperty(ref _code, value);
        }

        /// <summary>
        /// 学校名称
        /// </summary>
        [Description("名称")]
        public string Name {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        [JsonIgnore]
        public List<string> DefaultSelectList {
            get => defaultSelectList;
            set => SetProperty(ref defaultSelectList, value);
        }
    }
}
