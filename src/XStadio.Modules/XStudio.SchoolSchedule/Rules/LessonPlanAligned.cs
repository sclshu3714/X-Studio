using XStudio.SchoolSchedule.Enums;

namespace XStudio.SchoolSchedule.Rules {

    /// <summary>
    /// 教案齐平
    ///     指定班级的当前课程，其它班级当天也必须上这门课程
    /// </summary>
    public class LessonPlanAligned : IRule {

        public LessonPlanAligned(PriorityMode priority, ClassCourseRule course)
           : base(priority, RuleMode.Course, RuleType.LessonPlanAligned) {
        }
    }
}