using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.App.Models.Data {
    public class TimePeriod {
        /// <summary>
        /// 序号
        /// </summary>
        [JsonProperty("order")]
        public int Order { get; set; } = 0;

        /// <summary>
        /// 时段编号
        /// </summary>
        [Description("编码")]
        [JsonProperty("code")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 学校名称
        /// </summary>
        [Description("名称")]
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
    }
}
