using XStudio.SchoolSchedule.Enums;

namespace XStudio.SchoolSchedule.Rules {

    /// <summary>
    /// 单双周
    /// </summary>
    public class SingleOrBiweekly : IRule {

        public SingleOrBiweekly() :
            base(PriorityMode.Highest, RuleMode.Course, RuleType.SingleOrBiweekly) {
            SingleWeekly = null;
            BiWeekly = null;
            Type = RuleType.SingleOrBiweekly;
            Mode = RuleMode.Course;
            RangeType = ActionRangeType.Class;
            Code = "";
        }

        /// <summary>
        /// 课时,单周或者双周分别占classHour的一半
        /// </summary>
        public SingleOrBiweekly(PriorityMode priority, ClassCourseRule singleWeekly, ClassCourseRule biWeekly)
            : base(priority, RuleMode.Course, RuleType.SingleOrBiweekly) {
            SingleWeekly = singleWeekly;
            BiWeekly = biWeekly;
            Type = RuleType.SingleOrBiweekly;
            Mode = RuleMode.Course;
            Priority = priority;
            RangeType = ActionRangeType.Class;
            Code = $"{singleWeekly.Code};{biWeekly.Code}";
        }

        /// <summary>
        /// 单双周
        /// </summary>
        /// <param name="priority">优先级</param>
        /// <param name="singleWeekly">单周课程</param>
        /// <param name="biWeekly">双周课程</param>
        /// <param name="actionRange">作用范围</param>
        /// <param name="classHour">课时,单周或者双周分别占classHour的一半</param>
        public SingleOrBiweekly(PriorityMode priority, ClassCourseRule singleWeekly, ClassCourseRule biWeekly, List<string> actionRange, float classHour = 1) :
            base(priority, RuleMode.Course, RuleType.SingleOrBiweekly, ActionRangeType.Class, actionRange) {
            SingleWeekly = singleWeekly;
            BiWeekly = biWeekly;
            ClassHour = classHour;
            Code = $"{singleWeekly.Code};{biWeekly.Code}";
        }

        /// <summary>
        /// 显示名称
        /// </summary>
        public override string DisplayName {
            get {
                if(SingleWeekly != null && BiWeekly != null) {
                    return $"{SingleWeekly.DisplayName}|{BiWeekly.DisplayName}\r\n({GetDescription(Type)})";
                }
                return "无";
            }
        }

        /// <summary>
        /// 课程编号，主要用于快速识别和查询
        /// </summary>
        public override string Code { get; set; } = string.Empty;

        /// <summary>
        /// 单周课程
        /// </summary>
        public ClassCourseRule SingleWeekly { get; set; }

        /// <summary>
        /// 双周课程
        /// </summary>
        public ClassCourseRule BiWeekly { get; set; }
    }
}