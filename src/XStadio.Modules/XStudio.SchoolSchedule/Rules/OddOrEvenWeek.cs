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
    public class OddOrEvenWeek : IRule
    {
        public OddOrEvenWeek(int priority, ClassCourse oddWeek, ClassCourse evenWeek) {
            Priority = priority;
            OddWeek = oddWeek;
            EvenWeek = evenWeek;
        }

        public int Priority { get; set; }
        /// <summary>
        /// 单周课程
        /// </summary>
        public ClassCourse OddWeek { get; set; }

        /// <summary>
        /// 双周课程
        /// </summary>
        public ClassCourse EvenWeek { get; set; }

        /// <summary>
        /// 作用类型
        /// </summary>
        public RuleMode @Mode { get; set; } = RuleMode.Course;
    }
}
