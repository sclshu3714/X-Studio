using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.School.Timetable.Models {
    public class TimePeriod {
        /// <summary>
        /// 序号
        /// </summary>
        [Description("序号")]
        public int Order { get; set; } = 0;

        /// <summary>
        /// 时段编号
        /// </summary>
        [Description("编码")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 学校名称
        /// </summary>
        [Description("名称")]
        public string Name { get; set; } = string.Empty;
    }
}
