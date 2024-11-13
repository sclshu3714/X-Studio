using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XStudio.App.Models.Enums;

namespace XStudio.App.ViewModel.Module.Schools {
    public class ScheduleViewModel : ViewModelBase {
        [Description("序号")]
        public int Order { get; set; } = 0;
        /// <summary>
        /// 节次表编号，主要用于快速识别和查询
        /// </summary>
        [Description("编号")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 节次表名称
        /// </summary>
        [Description("节次方案名称")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 学段(幼儿园、学前班、小学、初中、高中、大学、研究生、博士生、其他)
        /// </summary>
        [Description("学段")]
        public EducationLevel Period { get; set; } = EducationLevel.Other;

        /// <summary>
        /// 年级 - 幼儿园（大班、小班）；小学（一年级、二年级、三年级、四年级,五年级）；初中（六年级、七年级、八年级、九年级）；高中（高一、高二、高三）；大学（大一、大二、大三、大四）；研究生（硕士研究生、博士研究生）；博士生（博士一、博士二、博士三）；其他
        /// </summary>
        [Description("年级")]
        public string Grade { get; set; } = string.Empty;

        /// <summary>
        /// 学年(2024-2025)
        /// </summary>
        [Description("学年")]
        public string SchoolYear { get; set; } = string.Empty;

        /// <summary>
        /// 学期(上学期/下学期、第一学期/第二学期)
        /// </summary>
        [Description("学期")]
        public string Semester { get; set; } = string.Empty;


        /// <summary>
        /// 默认周一 - 周日,当然也可以控制周日 - 周六,周六 - 周五等
        /// </summary>
        [Description("布局节次表")]
        public List<DayOfWeek> LayoutOfWeek { get; set; } = new List<DayOfWeek>() {
             DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
             DayOfWeek.Friday, DayOfWeek.Saturday,DayOfWeek.Sunday
        };
    }
}
