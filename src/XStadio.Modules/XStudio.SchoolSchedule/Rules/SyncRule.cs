using XStudio.SchoolSchedule.Enums;

namespace XStudio.SchoolSchedule.Rules {

    /// <summary>
    /// 同步规则
    ///     两个老师必须同时上课
    /// </summary>
    public class SyncRule : IRule {

        public SyncRule(PriorityMode priority, ClassCourseRule CourseA, ClassCourseRule CourseB)
            : base(priority, RuleMode.Course, RuleType.Sync) {
        }
    }
}