using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using XStudio.Common;
using XStudio.Schools.Timetable.Attributes;

namespace XStudio.Schools.Timetable
{
    /// <summary>
    /// 节
    /// </summary>
    public class Section : AuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 需要,主要用于排序
        /// </summary>
        [DbDescription("序号")]
        public int Order { get; set; } = 0;

        /// <summary>
        /// 节次编号
        /// </summary>
        /// <example>
        /// Section1
        /// </example>
        [DbDescription("编码")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 节次名称
        /// </summary>
        /// <example>
        /// 第一节
        /// </example>
        [DbDescription("名称")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 是否是授课|非授课,默认为true,当设置为false时指非授课
        /// </summary>
        [DbDescription("是否是授课")]
        public bool IsTeaching { get; set; } = true;

        /// <summary>
        /// 是否是自习|非授课,默认为false,当设置为true时是自习
        /// 当IsTeaching = true时及授课时才验证是否是自习
        /// </summary>
        [DbDescription("是否是自习")]
        [SelfStudyIfTeaching]
        public bool IsSelfStudy { get; set; } = false;

        /// <summary>
        /// 是否是通栏,默认为false,当设置为true时,该节次将贯穿所有选择的天
        /// 当IsTeaching = false时及非授课时才验证通栏
        /// </summary>
        [DbDescription("是否是通栏")]
        [BannerIfTeaching]
        public bool IsBanner {  get; set; } = false;

        /// <summary>
        /// 节次开始时间
        /// </summary>
        [DbDescription("节次开始时间")]
        public TimeSpan? StartTime { get; set; }

        /// <summary>
        /// 节次结束时间
        /// </summary>
        [DbDescription("节次结束时间")]
        public TimeSpan? EndTime { get; set; }

        /// <summary>
        /// "数据有效标识：A 正常 | E 异常 | S 停用 | D 删除
        /// </summary>

        [DbDescription("数据有效标识")]
        public ValidStateType ValidState { get; set; } = ValidStateType.A;

        /// <summary>
        /// 时段编码
        /// </summary>
        [DbDescription("时段编码")]
        public string PeriodCode { get; set; } = string.Empty;

        /// <summary>
        /// 节次编码
        /// </summary>
        [DbDescription("节次编码")]
        public string ScheduleCode { get; set; } = string.Empty;

        /// <summary>
        /// 时段
        /// </summary>
        /// <example>
        /// Morning
        /// </example>
        public virtual TimePeriod? Period { get; set; }

        /// <summary>
        /// 节次方案
        /// </summary>
        public virtual Schedule? @Schedule { get; set; }
    }
}
