using XStudio.SchoolSchedule.Enums;

namespace XStudio.SchoolSchedule.Rules {

    /// <summary>
    /// 合班
    /// </summary>
    public class JointClassTeaching : IRule {

        /// <summary>
        /// 合班课
        /// </summary>
        /// <param name="priority"></param>
        /// <param name="rules"></param>
        /// <param name="classes"></param>
        /// <param name="classHour"></param>
        public JointClassTeaching(PriorityMode priority,
                                  ClassCourseRule rule,
                                  List<SchoolClass> classes,
                                  int classHour = 1)
                : base(priority, RuleMode.Teacher, RuleType.JointClassTeaching) {
            ClassHour = classHour;
            Course = rule;
            Classes = classes;
            Priority = priority;
            RangeType = ActionRangeType.Class;
            Mode = RuleMode.Teacher;
            Type = RuleType.JointClassTeaching;
            Code = rule.Code;
        }

        /// <summary>
        /// 显示名称
        /// </summary>
        public override string DisplayName {
            get {
                if(Course != null) {
                    return $"{Course.Name}\r\n({GetDescription(Type)})";
                }
                return "无";
            }
        }

        /// <summary>
        /// 合并课程
        /// </summary>
        public ClassCourseRule Course { get; set; }

        /// <summary>
        /// 合并班级
        /// </summary>
        public List<SchoolClass> Classes { get; set; } = new List<SchoolClass>();
    }
}