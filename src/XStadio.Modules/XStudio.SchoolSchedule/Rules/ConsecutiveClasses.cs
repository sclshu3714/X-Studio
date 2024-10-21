using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.SchoolSchedule.Rules {
    public class ConsecutiveClasses : IRule {
        /// <summary>
        /// 课时,单周或者双周分别占classHour的一半
        /// </summary>
        public ConsecutiveClasses(PriorityMode priority, ClassCourseRule classCourse) {
            Priority = priority;
            ClassCourse = classCourse;
            Type = RuleType.ContinuousClasses;
            RangeType = ActionRangeType.Class;
            Mode = RuleMode.Course;
            Id = classCourse.Id;
        }
        /// <summary>
        /// 单双周
        /// </summary>
        /// <param name="priority">优先级</param>
        /// <param name="singleWeekly">单周课程</param>
        /// <param name="biWeekly">双周课程</param>
        /// <param name="actionRange">作用范围</param>
        /// <param name="classHour">课时,单周或者双周分别占classHour的一半</param>
        public ConsecutiveClasses(PriorityMode priority, ClassCourseRule classCourse, List<string> actionRange, float classHour = 1) :
            base(priority, RuleMode.Course, RuleType.ContinuousClasses, ActionRangeType.Class, actionRange) {
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
    }
}
