using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.SchoolSchedule.Rules
{
    /// <summary>
    /// 单双周
    /// </summary>
    public class SingleOrBiweeklyRule : IRule
    {
        public SingleOrBiweeklyRule(int priority, ClassCourse singleWeekly, ClassCourse biWeekly)
        {
            Priority = priority;
            SingleWeekly = singleWeekly;
            BiWeekly = biWeekly;
        }
        /// <summary>
        /// 单周课程
        /// </summary>
        public ClassCourse SingleWeekly { get; set; }

        /// <summary>
        /// 双周课程
        /// </summary>
        public ClassCourse BiWeekly { get; set; }
    }
}
