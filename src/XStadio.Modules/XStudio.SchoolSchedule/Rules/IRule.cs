using System.ComponentModel;
using System.Reflection;
using XStudio.SchoolSchedule.Enums;

namespace XStudio.SchoolSchedule.Rules {

    /// <summary>
    /// 规则接口
    /// </summary>
    [Serializable]
    public class IRule {

        public IRule(string id) {
            Id = id;
        }

        public IRule(PriorityMode priority)
            : this(Guid.NewGuid().ToString()) {
            Priority = priority;
        }

        public IRule(PriorityMode priority, RuleMode mode)
            : this(priority) {
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
        /// 规则Id，唯一表示
        /// </summary>
        public string Id { get; private set; } = string.Empty;

        /// <summary>
        /// 课程代码
        /// </summary>
        public virtual string Code { get; set; } = string.Empty;

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
        [Description("作用范围类型")]
        public ActionRangeType RangeType { get; set; } = ActionRangeType.None;

        /// <summary>
        /// 作用范围：
        /// RangeType == 0 时，作用范围数量为0;
        /// RangeType == 1/2/3/4 时，作用范围记录范围类型对应的Id, 如果数量为0时表示取全部
        /// </summary>
        public List<string> ActionRange { get; set; } = new List<string>();

        /// <summary>
        /// 位置信息
        ///     key: 星期几(1-7)
        ///     value: 第几节课
        /// </summary>
        public Tuple<DayOfWeek, int>? Location { get; set; }

        /// <summary>
        /// 限制到节次的某个类型
        ///     默认限制到正课授课, 即SectionType.RegularClass
        ///     如果限制设置为None, 则不限制,允许放入(自习与正课授课)，不允许放入（活动、午休等非教学课）
        /// </summary>
        /// <example>
        ///     将一个语文课限制安排到早自习内，那么这个语文课就是早读语文
        /// </example>
        public SectionType RestrictType { get; set; } = SectionType.RegularClass;

        /// <summary>
        /// 获取属性的DescriptionAttribute注释
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string GetDescription<T>(T obj) {
            if(obj == null)
                return "未知";
            Type type = obj.GetType();
            var field = type.GetField($"{obj}");
            var descriptionAttribute = field?.GetCustomAttribute<DescriptionAttribute>();
            return descriptionAttribute?.Description ?? $"{obj}";
        }
    }
}