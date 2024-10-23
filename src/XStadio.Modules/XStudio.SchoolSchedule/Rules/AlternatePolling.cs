using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.SchoolSchedule.Rules {
    /// <summary>
    /// 交替轮巡
    /// </summary>
    public class AlternatePolling : IRule {
        /// <summary>
        /// 课时,单周或者双周分别占classHour的一半
        /// </summary>
        public AlternatePolling(PriorityMode priority, List<ClassCourseRule> rules, float classHour = 1) {
            ClassHour = classHour;
            PollingCourses = rules;
            Priority = priority;
            RangeType = ActionRangeType.Class;
            Mode = RuleMode.Course;
            Type = RuleType.AlternatePolling;
            Id = string.Join(";", rules.Select(r => r.Id));
            
        }

        /// <summary>
        /// 显示名称
        /// </summary>
        public override string DisplayName {
            get {
                if (PollingCourses != null && PollingCourses.Any()) {
                    return $"{string.Join("|", PollingCourses.Select(r => r.DisplayName))}\r\n({GetDescription(Type)})";
                }
                return "无";
            }
        }

        /// <summary>
        /// 轮巡的课程
        /// </summary>
        public List<ClassCourseRule> PollingCourses { get; set; } = new List<ClassCourseRule>();
    }
}
