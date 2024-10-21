using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
        /// 课程编号，主要用于快速识别和查询
        /// </summary>
        public virtual string Id { get; set; } = string.Empty;

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
        /// 作用范围：
        /// RangeType == 0 时，作用范围数量为0
        /// RangeType == 1 2 3 4 时，作用范围记录范围类型对应的Id, 如果数量为0时表示取全部
        /// </summary>
        public List<string> ActionRange { get; set; } = new List<string>();

        /// <summary>
        /// 位置信息
        /// </summary>
        public Tuple<DayOfWeek, int> Location;

        /// <summary>
        /// 获取属性的DescriptionAttribute注释
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetDescription<T>(T obj) {
            if (obj == null) return "未知";
            Type type = obj.GetType();
            var field = type.GetField($"{obj}");
            var descriptionAttribute = field?.GetCustomAttribute<DescriptionAttribute>();
            return descriptionAttribute?.Description ?? $"{obj}";
        }
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
    /// 规则类型(顺序表示规则顺序)
    /// </summary>
    public enum RuleType {
        /// <summary>
        /// 只能排[课程、教师]
        /// </summary>
        [Description("只能排")]
        CanOnlyArrange = 0,

        /// <summary>
        /// 不能排[课程、教师]
        /// </summary>
        [Description("不能排")]
        CannotBeArranged = 1,
        /// <summary>
        /// 连堂[课程]
        /// </summary>
        [Description("连堂")]
        ContinuousClasses = 2,

        /// <summary>
        /// 单周[课程]
        /// </summary>
        [Description("单周")]
        Single = 3,

        /// <summary>
        /// 双周[课程]
        /// </summary>
        [Description("双周")]
        Biweekly = 4,

        /// <summary>
        /// 单双周[课程]
        ///     两门课交替轮询
        /// </summary>
        [Description("单双周")]
        SingleOrBiweekly = 5,

        /// <summary>
        /// 交替轮询[课程]
        ///     多门课交替轮询
        /// </summary>
        [Description("交替轮询")]
        AlternatePolling = 6,
        
        /// <summary>
        /// 合班[教师]
        /// </summary>
        [Description("合班")]
        JointClassTeaching = 7,

        /// <summary>
        /// 集中备课[教师]
        /// </summary>
        [Description("集中备课")]
        CentralizedLessonPreparation = 8,

        /// <summary>
        /// 无规则
        /// </summary>
        [Description("无规则")]
        None = 9,

        /// <summary>
        /// 教案齐平[课程]
        /// </summary>
        [Description("教案齐平")]
        LessonPlanAligned = 10,

        /// <summary>
        /// 课程不相邻[课程]
        /// </summary>
        [Description("课程不相邻")]
        CoursesAreNotAdjacent = 11,

        /// <summary>
        /// 周内分散[课程、教师]
        /// </summary>
        [Description("周内分散")]
        DisperseWithinTheWeek = 12,

        /// <summary>
        /// 周内集中[教师]
        /// </summary>
        [Description("周内集中")]
        ConcentrationWithinTheWeek = 13,

        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        Unknown = 14,
    }

    /// <summary>
    /// 作用范围
    /// </summary>
    public enum ActionRangeType {
        [Description("无")]
        None = 0,
        [Description("课程")]
        Course = 1,
        [Description("教师")]
        Teacher = 2,
        [Description("班级")]
        Class = 3,
        [Description("场所")]
        Place = 4
    }
}
