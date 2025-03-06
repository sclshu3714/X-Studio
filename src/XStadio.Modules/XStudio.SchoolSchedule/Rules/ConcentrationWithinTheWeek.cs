using XStudio.SchoolSchedule.Enums;

namespace XStudio.SchoolSchedule.Rules {

    /// <summary>
    /// 周内集中
    ///     在周内集中上课
    /// </summary>
    public class ConcentrationWithinTheWeek : IRule {

        public ConcentrationWithinTheWeek(PriorityMode priority, ClassCourseRule course)
            : base(priority, RuleMode.Course, RuleType.ConcentrationWithinTheWeek) {
        }
    }
}