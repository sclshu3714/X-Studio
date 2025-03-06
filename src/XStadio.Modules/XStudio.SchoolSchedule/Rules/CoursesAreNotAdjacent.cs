using XStudio.SchoolSchedule.Constraints;
using XStudio.SchoolSchedule.Enums;

namespace XStudio.SchoolSchedule.Rules {

    /// <summary>
    /// 课程不能相邻
    ///      指定的课程不能在相邻的节次上课，相邻指的是中间无间隔的节次
    /// </summary>
    public class CoursesAreNotAdjacent : IConstraint {

        public CoursesAreNotAdjacent(PriorityMode priority, List<ClassCourseRule> classCourses)
            : base(priority, RuleMode.Course, RuleType.CoursesAreNotAdjacent) {
            ClassCourses = classCourses;
            Code = string.Join(",", classCourses.Select(c => c.Code));
        }

        /// <summary>
        /// 显示名称
        /// </summary>
        public override string DisplayName {
            get {
                switch(Mode) {
                    case RuleMode.Course:
                        return $"{string.Join(",", ClassCourses.Select(c => c.Name))}\r\n({GetDescription(Type)})";

                    case RuleMode.Teacher:
                        return $"{string.Join(",", ClassCourses.Select(c => c.TeacherName))}\r\n({GetDescription(Type)})";

                    default:
                        break;
                }
                return "无";
            }
        }

        /// <summary>
        /// 课程和老师信息
        /// </summary>
        public List<ClassCourseRule> ClassCourses { get; set; }
    }
}