using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
