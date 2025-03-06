using XStudio.SchoolSchedule.Enums;

namespace XStudio.SchoolSchedule.Rules {

    /// <summary>
    /// 集中备课
    ///     在集中备课教研组内的老师不能排课，只能参加集中备课教学。
    /// </summary>
    public class CentralizedLessonPreparation : IRule {

        public CentralizedLessonPreparation(PriorityMode priority,
                                            RuleMode mode,
                                            ClassCourseRule classCourse,
                                            Tuple<DayOfWeek, int> location)
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