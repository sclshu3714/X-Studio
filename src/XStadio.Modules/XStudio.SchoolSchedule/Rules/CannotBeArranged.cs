using XStudio.SchoolSchedule.Constraints;
using XStudio.SchoolSchedule.Enums;

namespace XStudio.SchoolSchedule.Rules {

    /// <summary>
    /// 不能排
    ///     指定位置(星期+节次)在指定班级(或者全部班级)内不能排指定的课程或者指定老师的课程
    /// </summary>
    public class CannotBeArranged : Constraint {

        public CannotBeArranged(PriorityMode priority, RuleMode mode, List<ClassCourseRule> classCourses, Tuple<DayOfWeek, int>? location)
            : base(priority, mode, RuleType.CannotBeArranged) {
            Location = location;
            ClassCourses = classCourses;
            Code = string.Join(",", classCourses.Select(x => x.Code));
        }

        /// <summary>
        /// 显示名称
        /// </summary>
        public override string DisplayName {
            get {
                switch(Mode) {
                    case RuleMode.Course:
                        return $"{string.Join(",", ClassCourses.Select(x => x.DisplayName))}\r\n({GetDescription(Type)})";

                    case RuleMode.Teacher:
                        return $"{string.Join(",", ClassCourses.Select(x => x.TeacherName))}\r\n({GetDescription(Type)})";

                    default:
                        break;
                }
                return "无";
            }
        }

        /// <summary>
        /// 课程和老师信息
        /// </summary>
        public List<ClassCourseRule> ClassCourses { get; set; } = new List<ClassCourseRule>();
    }
}