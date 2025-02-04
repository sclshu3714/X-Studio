using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.SchoolSchedule.Rules {
    /// <summary>
    /// 课程不能相邻
    ///      指定的课程不能在相邻的节次上课，相邻指的是中间无间隔的节次
    /// </summary>
    public class CoursesAreNotAdjacent : IRule {
        public CoursesAreNotAdjacent(PriorityMode priority, ClassCourseRule course) 
            : base(priority, RuleMode.Course, RuleType.CoursesAreNotAdjacent) {
        }
    }
}
