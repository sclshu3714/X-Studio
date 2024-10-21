using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.SchoolSchedule.Rules {
    /// <summary>
    /// 单双周
    /// </summary>
    public class SingleOrBiweekly : IRule {
        /// <summary>
        /// 课时,单周或者双周分别占classHour的一半
        /// </summary>
        public SingleOrBiweekly(PriorityMode priority, ClassCourseRule singleWeekly, ClassCourseRule biWeekly) {
            SingleWeekly = singleWeekly;
            BiWeekly = biWeekly;
            Type = RuleType.SingleOrBiweekly;
            Mode = RuleMode.Course;
            Priority = priority;
            RangeType = ActionRangeType.Class;
        }
        /// <summary>
        /// 单双周
        /// </summary>
        /// <param name="priority">优先级</param>
        /// <param name="singleWeekly">单周课程</param>
        /// <param name="biWeekly">双周课程</param>
        /// <param name="actionRange">作用范围</param>
        /// <param name="classHour">课时,单周或者双周分别占classHour的一半</param>
        public SingleOrBiweekly(PriorityMode priority, ClassCourseRule singleWeekly, ClassCourseRule biWeekly, List<string> actionRange, float classHour = 1) :
            base(priority, RuleMode.Course, RuleType.SingleOrBiweekly, ActionRangeType.Class, actionRange) {
            SingleWeekly = singleWeekly;
            BiWeekly = biWeekly;
            ClassHour = classHour;
        }
        /// <summary>
        /// 显示名称
        /// </summary>
        public override string DisplayName {
            get {
                if (SingleWeekly != null && BiWeekly != null) {
                    return $"{SingleWeekly.DisplayName}|{BiWeekly.DisplayName}\r\n({GetDescription(Type)})";
                }
                return "无";
            }
        }

        /// <summary>
        /// 单周课程
        /// </summary>
        public ClassCourseRule SingleWeekly { get; set; }

        /// <summary>
        /// 双周课程
        /// </summary>
        public ClassCourseRule BiWeekly { get; set; }
    }
}
