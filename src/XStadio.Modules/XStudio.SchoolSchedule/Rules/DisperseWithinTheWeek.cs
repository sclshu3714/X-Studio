using XStudio.SchoolSchedule.Enums;

namespace XStudio.SchoolSchedule.Rules {

    /// <summary>
    /// 周内分散规则
    ///     指定的课程必须在同一周内的不同时间段(或者指定的上午或者下午)内安排。
    ///     分散在一周内
    /// </summary>
    public class DisperseWithinTheWeek : IRule {

        public DisperseWithinTheWeek(PriorityMode priority, ClassCourseRule course)
           : base(priority, RuleMode.Course, RuleType.DisperseWithinTheWeek) {
        }
    }
}