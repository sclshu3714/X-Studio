using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.SchoolSchedule
{
    /// <summary>
    /// 单双周
    /// </summary>
    public class OddOrEvenWeek : IRule
    {
        /// <summary>
        /// 单周课程
        /// </summary>
        public ClassCourse OddWeek { get; set; }

        /// <summary>
        /// 双周课程
        /// </summary>
        public ClassCourse EvenWeek { get; set; }
    }
}
