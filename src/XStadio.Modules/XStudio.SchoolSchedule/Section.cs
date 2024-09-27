using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XStudio.SchoolSchedule.Rules;

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
        /// 教学周
        ///     从第0周开始计数,但是教学周从1开始，0为模版
        ///     当作为模版时数据为0, 既是设计课表是不考虑教学周，只有发布课表后生成学期课表才会有教学周
        /// </summary>
        public int TeachingWeek { get; set; } = 0;

        /// <summary>
        /// 星期(Column)
        /// </summary>
        public DayOfWeek Day { get; set; } = DayOfWeek.Monday;

        /// <summary>
        /// 时段
        /// </summary>
        public string TimePeriod { get; set; } = "上午";

        /// <summary>
        /// 节次
        /// </summary>
        public int Index { get; set; } = 1;

        /// <summary>
        /// 节次代码
        /// </summary>
        /// <example>
        /// 规则：一周最多7天，但是一天的课程可能超过10节但是不会超过99节
        /// 00101 => 星期一第1节
        /// 00102 => 星期一第2节
        /// 00201 => 星期二第1节
        /// 00510 => 星期五第10节
        /// 00712 => 星期天第12节
        /// 01712 => 第一周星期天第12节
        /// </example>
        public string Code { get; set; } = "00101";

        /// <summary>
        /// 节次名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 节次类型(早自习 正课授课 课间活动 午自习 午休 晚自习)
        /// </summary>
        public SectionType @Type { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public List<SectionContent> Contents { get; set; } = new List<SectionContent>();

        /// <summary>
        /// 节次状态
        /// </summary>
        public SectionStatus @Status { get; set; } = SectionStatus.Normal;

        /// <summary>
        /// 生成节次代码
        /// </summary>
        /// <param name="index">第几节</param>
        /// <param name="day">星期几，如果不设置，默认使用当前节次的星期</param>
        /// <param name="week">第几教学周，默认为0</param>
        /// <returns></returns>
        public void SetSectionCode(int index, DayOfWeek day, int week = 0)
        { 
            this.Day = day;
            Index = index;
            Code = $"{week:D2}{(int)day}{index:D2}";
        }

        /// <summary>
        /// 添加内容
        /// 添加完成后注意排序
        /// Contents.Sort((x, y) => x.Index.CompareTo(y.Index));
        /// </summary>
        /// <param name="content"></param>
        public void AddSectionContent(SectionContent content)
        {
            Contents.Add(content);
        }
    }

    /// <summary>
    /// 节次内容
    ///     显示: 课程、教师、场所、时间、规则
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

    /// <summary>
    /// 节次类型
    /// </summary>
    public enum SectionType
    {
        MorningStudy = 0, // 早间自习  
        RegularClass,     // 正课授课
        BreakExercise,    // 课间活动
        AfternoonStudy,   // 午间自习
        NoonBreak,        // 午休时段
        EveningStudy,     // 晚间自习
    }

    /// <summary>
    /// 节次状态
    /// </summary>
    public enum SectionStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal,
        /// <summary>
        /// 锁定
        /// </summary>
        Lock,
        /// <summary>
        /// 禁用
        /// </summary>
        Disable
    }
}
