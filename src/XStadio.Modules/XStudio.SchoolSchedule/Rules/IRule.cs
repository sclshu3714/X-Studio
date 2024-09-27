using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.SchoolSchedule.Rules
{
    /// <summary>
    /// 规则接口
    /// </summary>
    public class IRule
    {
        /// <summary>
        /// 优先级，1-N, 1 最高, N最低
        /// </summary>
        [Description("优先级")]
        public int Priority { get; set; }

        /// <summary>
        /// 作用类型
        /// </summary>
        [Description("作用类型")]
        public RuleMode @Mode { get; set; }

        /// <summary>
        /// 规则类型
        /// </summary>
        [Description("规则类型")]
        public RuleType @Type { get; set; }

        /// <summary>
        /// 作用范围类型
        /// </summary>
        [Description("范围类型")]
        public ActionRangeType RangeType { get; set; }

        /// <summary>
        /// 作用范围，Id集合
        /// </summary>
        public List<string> ActionRange {  get; set; } = new List<string>();
    }

    /// <summary>
    /// 作用类型
    /// </summary>
    public enum RuleMode
    {
        /// <summary>
        /// 课程
        /// </summary>
        [Description("课程")]
        Course,
        /// <summary>
        /// 教师
        /// </summary>
        [Description("教师")]
        Teacher,
    }

    /// <summary>
    /// 规则类型
    /// </summary>
    public enum RuleType
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        Unknown = 0,
        /// <summary>
        /// 只能排[课程、教师]
        /// </summary>
        [Description("只能排")]
        CanOnlyArrange = 1,
        /// <summary>
        /// 不能排[课程、教师]
        /// </summary>
        [Description("不能排")]
        CannotBeArranged,
        /// <summary>
        /// 单双周[课程]
        ///     两门课交替轮询
        /// </summary>
        [Description("单双周")]
        SingleOrBiweekly,
        /// <summary>
        /// 交替轮询[课程]
        ///     多门课交替轮询
        /// </summary>
        [Description("交替轮询")]
        AlternatePolling,
        /// <summary>
        /// 连堂[课程]
        /// </summary>
        [Description("连堂")]
        ContinuousClasses,
        /// <summary>
        /// 合班[教师]
        /// </summary>
        [Description("合班")]
        JointClassTeaching,
        /// <summary>
        /// 教案齐平[课程]
        /// </summary>
        [Description("教案齐平")]
        LessonPlanAligned,
        /// <summary>
        /// 课程不相邻[课程]
        /// </summary>
        [Description("课程不相邻")]
        CoursesAreNotAdjacent,
        /// <summary>
        /// 周内分散[课程、教师]
        /// </summary>
        [Description("周内分散")]
        DisperseWithinTheWeek,
        /// <summary>
        /// 周内集中[教师]
        /// </summary>
        [Description("周内集中")]
        ConcentrationWithinTheWeek,
        /// <summary>
        /// 集中备课[教师]
        /// </summary>
        [Description("集中备课")]
        CentralizedLessonPreparation,
    }

    /// <summary>
    /// 作用范围
    /// </summary>
    public enum ActionRangeType
    {
        [Description("课程")]
        Course,
        [Description("教师")]
        Teacher,
        [Description("班级")]
        Class,
        [Description("场所")]
        Place
    }
}
