using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XStudio.SchoolSchedule;

namespace XStudio.School.Timetable.Models
{
    public class TimetableRow
    {
        /// <summary>
        /// 时段
        /// </summary>
        public string TimeSlot { get; set; }

        /// <summary>
        /// 节次
        /// </summary>
        public int Period { get; internal set; }
        /// <summary>
        /// 节次名称
        /// </summary>
        public string PeriodName => $"第 {Period} 节";

        /// <summary>
        /// 内容
        /// </summary>
        public string Day1 { get; set; }
        public string Day2 { get; set; }
        public string Day3 { get; set; }    
        public string Day4 { get; set; }
        public string Day5 { get; set; }
        public string Day6 { get; set; }
        public string Day7 { get; set; }

        public bool IsMerged { get; set; } // 是否合并
        public int RowSpan { get; set; }   // 行跨度
        public int ColSpan { get; set; }   // 列跨度

    }
}
