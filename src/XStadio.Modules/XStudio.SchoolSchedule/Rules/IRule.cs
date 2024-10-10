using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.SchoolSchedule.Rules {
    /// <summary>
    /// 规则接口
    /// </summary>
    public class IRule {
        public IRule() {

        }

        public IRule(PriorityMode priority, RuleMode mode)
            : this() {
            Priority = priority;
            @Mode = mode;
        }
        public IRule(PriorityMode priority, RuleMode mode, RuleType type)
            : this(priority, mode) {
            @Type = type;
        }

        public IRule(PriorityMode priority, RuleMode mode, RuleType type, ActionRangeType rangeType, List<string> actionRange)
            : this(priority, mode, type) {
            RangeType = rangeType;
            ActionRange = actionRange;
        }

        /// <summary>
        /// 使用课时,占用课时,如果为0,不限制
        /// </summary>
        [Description("课时")]
        public virtual float ClassHour { get; set; } = 1;

        /// <summary>
        /// 显示名称
        /// </summary>
        [Description("显示名称")]
        public virtual string DisplayName { get; set; } = "无名称";

        /// <summary>
        /// 优先级，默认中
        /// </summary>
        [Description("优先级")]
        public PriorityMode Priority { get; set; } = PriorityMode.Medium;

        /// <summary>
        /// 作用类型
        /// </summary>
        [Description("作用类型")]
        public RuleMode @Mode { get; set; } = RuleMode.None;

        /// <summary>
        /// 规则类型
        /// </summary>
        [Description("规则类型")]
        public RuleType @Type { get; set; } = RuleType.None;

        /// <summary>
        /// 作用范围类型
        /// </summary>
        [Description("范围类型")]
        public ActionRangeType RangeType { get; set; } = ActionRangeType.None;

        /// <summary>
        /// 作用范围，Id集合
        /// </summary>
        public List<string> ActionRange { get; set; } = new List<string>();
    }

    public enum PriorityMode {
        [Description("最高")]
        Highest = 2,
        [Description("高")]
        High = 1,
        [Description("中")]
        Medium = 0,
        [Description("低")]
        Low = -1,
        [Description("最低")]
        Lowest = -2
    }

    /// <summary>
    /// 作用类型
    /// </summary>
    public enum RuleMode {
        [Description("无")]
        None = 0,
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
    public enum RuleType {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        Unknown = 0,
        /// <summary>
        /// 无规则
        /// </summary>
        [Description("无规则")]
        None = 1,
        /// <summary>
        /// 只能排[课程、教师]
        /// </summary>
        [Description("只能排")]
        CanOnlyArrange = 2,
        /// <summary>
        /// 不能排[课程、教师]
        /// </summary>
        [Description("不能排")]
        CannotBeArranged,
        /// <summary>
        /// 单周[课程]
        /// </summary>
        [Description("单周")]
        Single,
        /// <summary>
        /// 双周[课程]
        /// </summary>
        [Description("双周")]
        Biweekly,
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
    public enum ActionRangeType {
        [Description("无")]
        None = 0,
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
