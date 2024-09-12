using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.SchoolSchedule
{
    /// <summary>
    /// 课程表中的节次
    /// </summary>
    /// <example>
    /// 第一周星期一上午第二节
    /// </example>
    public class Section
    {
        /// <summary>
        /// 教学周(从第1周开始计数)
        /// </summary>
        public int TeachingWeek { get; set; } = 1;
        /// <summary>
        /// 星期
        /// </summary>
        public DayOfWeek Week { get; set; } = DayOfWeek.Monday;

        /// <summary>
        /// 时段
        /// </summary>
        public string TimePeriod { get; set; } = "上午";

        /// <summary>
        /// 第几节(从1开始计数)
        /// </summary>
        /// <example>
        /// 规则：一周最多7天，但是一天的课程可能超过10节但是不会超过99节
        /// 101 => 周1第1节
        /// 102 => 周1第2节
        /// 201 => 周2第1节
        /// 510 => 周5第10节
        /// </example>
        public int Index { get; set; } = 1;

        /// <summary>
        /// 节次名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 内容
        /// </summary>
        public List<SectionContent> Contents { get; set; } = new List<SectionContent>();

    }

    /// <summary>
    /// 节次内容
    /// </summary>
    public class SectionContent : IContent<IRule>
    { 
        /// <summary>
        /// 序列号，控制显示顺序
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 周间隔
        /// </summary>
        /// <example>
        /// 常规课：每周都需要上         => 0
        /// 单双周：单周或者双周才上的课 => 单周 1 | 双周 2
        /// 轮巡周：每间隔多少周轮巡1次  => 轮巡次数 n， n <= 学期周 最少一个学期上1次， 如设置为3，那么每3周上一次课，该节次内容可以排3个课程，轮巡上课
        /// </example>
        public int WeeklyInterval { get; set; } = 0;
    }
}
