using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.App.ViewModel.Module.Schools {
    public class SectionViewModel : ViewModelBase {
        private int _order = 0;
        private string _code = string.Empty;
        private string _name = string.Empty;

        private List<string> defaultSelectList = new List<string>() { "早晨", "上午", "中午", "下午", "晚上" };
        private string _period;
        private string _periodCode;

        /// <summary>
        /// 序号
        /// </summary>
        [Description("序号")]
        [JsonProperty("order")]
        public int Order {
            get => _order;
            set => SetProperty(ref _order, value);
        }

        /// <summary>
        /// 节次编号
        /// </summary>
        [Description("编码")]
        [JsonProperty("code")]
        public string Code {
            get => _code;
            set => SetProperty(ref _code, value);
        }

        /// <summary>
        /// 时段编码
        /// </summary>
        [Description("名称")]
        [JsonProperty("periodCode")]
        public string PeriodCode {
            get => _periodCode;
            set => SetProperty(ref _periodCode, value);
        }

        /// <summary>
        /// 时段
        /// </summary>
        [Description("时段")]
        [JsonProperty("period")]
        public string Period {
            get => _period;
            set => SetProperty(ref _period, value);
        }

        /// <summary>
        /// 节次名称
        /// </summary>
        [Description("名称")]
        [JsonProperty("name")]
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
