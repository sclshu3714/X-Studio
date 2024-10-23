using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.SchoolSchedule.Rules {
    public class ConsecutiveClasses : IRule {
        /// <summary>
        /// 连堂课
        /// </summary>
        /// <param name="priority"></param>
        /// <param name="classCourse"></param>
        public ConsecutiveClasses(PriorityMode priority, ClassCourseRule classCourse) {
            Priority = priority;
            ClassCourse = classCourse;
            Type = RuleType.ConsecutiveClasses;
            RangeType = ActionRangeType.Class;
            Mode = RuleMode.Course;
            Id = classCourse.Id;
        }
        /// <summary>
        /// 连堂课
        /// </summary>
        /// <param name="priority"></param>
        /// <param name="classCourse"></param>
        /// <param name="actionRange"></param>
        /// <param name="classHour"></param>
        public ConsecutiveClasses(PriorityMode priority, ClassCourseRule classCourse, List<string> actionRange, float classHour = 1) :
            base(priority, RuleMode.Course, RuleType.ConsecutiveClasses, ActionRangeType.Class, actionRange) {
            ClassCourse = classCourse;
            ClassHour = classHour;
            Id = classCourse.Id;
        }
        /// <summary>
        /// 显示名称
        /// </summary>
        public override string DisplayName {
            get {
                if (ClassCourse != null) {
                    return $"{ClassCourse.DisplayName}\r\n({GetDescription(Type)})";
                }
                return "无";
            }
        }

        /// <summary>
        /// 课程编号，主要用于快速识别和查询
        /// </summary>
        public override string Id { get; set; } = string.Empty;

        /// <summary>
        /// 单周课程
        /// </summary>
        public ClassCourseRule ClassCourse { get; set; }

        /// <summary>
        /// 连堂
        /// </summary>
        public List<int> Periods { get; set; } = new List<int>();
    }
}
