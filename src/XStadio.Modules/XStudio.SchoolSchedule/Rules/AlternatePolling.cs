using XStudio.SchoolSchedule.Enums;

namespace XStudio.SchoolSchedule.Rules {

    /// <summary>
    /// 交替轮巡
    /// </summary>
    public class AlternatePolling : IRule {

        /// <summary>
        /// 课时,单周或者双周分别占classHour的一半
        ///     多个课程在学期内按照顺序轮巡上课
        ///     如：[游泳->篮球->乒乓球->足球]
        /// </summary>
        public AlternatePolling(PriorityMode priority, List<ClassCourseRule> rules, float classHour = 1)
               : base(priority, RuleMode.Course, RuleType.AlternatePolling) {
            ClassHour = classHour;
            PollingCourses = rules;
            Priority = priority;
            RangeType = ActionRangeType.Class;
            Mode = RuleMode.Course;
            Type = RuleType.AlternatePolling;
            if(rules != null && rules.Any()) {
                Code = string.Join(";", rules.Select(r => r.Code));
            }
        }

        /// <summary>
        /// 显示名称
        /// </summary>
        public override string DisplayName {
            get {
                if(PollingCourses != null && PollingCourses.Count == 2 && Interval == 1) {
                    return $"{string.Join("|", PollingCourses.Select(r => r.DisplayName))}\r\n({GetDescription(RuleType.SingleOrBiweekly)})";
                }
                else if(PollingCourses != null && PollingCourses.Any()) {
                    return $"{string.Join("|", PollingCourses.Select(r => r.DisplayName))}\r\n({GetDescription(Type)})";
                }
                return "无";
            }
        }

        /// <summary>
        /// 交替轮巡间隔
        ///     间隔默认为1,即每周轮巡一次
        ///     间隔为N,则每N周轮巡一次
        /// </summary>
        public int Interval { get; set; } = 1;

        /// <summary>
        /// 轮巡的课程
        /// </summary>
        public List<ClassCourseRule> PollingCourses { get; set; } = new List<ClassCourseRule>();
    }
}