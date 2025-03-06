using XStudio.SchoolSchedule.Constraints;
using XStudio.SchoolSchedule.Enums;

namespace XStudio.SchoolSchedule.Rules {

    /// <summary>
    /// 只能排
    ///     指定位置(星期+节次)在指定班级(或者全部班级)内只能排指定的课程或者指定老师的课程
    /// </summary>
    public class CanOnlyArrange : Constraint {

        /// <summary>
        /// 只能排
        /// </summary>
        /// <param name="priority">权重</param>
        /// <param name="mode">作用类型(课程 / 教师)</param>
        /// <param name="classCourse">课程</param>
        /// <param name="location">位置</param>
        public CanOnlyArrange(PriorityMode priority, RuleMode mode, ClassCourseRule classCourse, Tuple<DayOfWeek, int> location)
            : base(priority, mode, RuleType.CanOnlyArrange) {
            Location = location;
            ClassCourse = classCourse;
            Code = classCourse.Code;
        }

        /// <summary>
        /// 显示名称
        /// </summary>
        public override string DisplayName {
            get {
                switch(Mode) {
                    case RuleMode.Course:
                        return $"{ClassCourse.DisplayName}\r\n({GetDescription(Type)})";

                    case RuleMode.Teacher:
                        return $"{ClassCourse.TeacherName}\r\n({GetDescription(Type)})";

                    default:
                        break;
                }
                return "无";
            }
        }

        /// <summary>
        /// 课程和老师信息
        /// </summary>
        public ClassCourseRule ClassCourse { get; set; }
    }
}