using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using XStudio.SchoolSchedule;
using XStudio.SchoolSchedule.Rules;

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
        /// 单元格集合
        /// </summary>
        public ObservableCollection<TimetableCell> Cells { get; set; } = new ObservableCollection<TimetableCell>();

        /// <summary>
        /// 内容
        /// </summary>
        public TimetableCell Day1 { get; set; }
        public Brush Day1Foreground { get; set; } = Brushes.Black;
        public Brush Day1Background { get; set; } = Brushes.White;
        public TimetableCell Day2 { get; set; }

        public Brush Day2Foreground { get; set; } = Brushes.Black;
        public Brush Day2Background { get; set; } = Brushes.White;
        public TimetableCell Day3 { get; set; }

        public Brush Day3Foreground { get; set; } = Brushes.Black;
        public Brush Day3Background { get; set; } = Brushes.White;
        public TimetableCell Day4 { get; set; }

        public Brush Day4Foreground { get; set; } = Brushes.Black;
        public Brush Day4Background { get; set; } = Brushes.White;
        public TimetableCell Day5 { get; set; }

        public Brush Day5Foreground { get; set; } = Brushes.Black;
        public Brush Day5Background { get; set; } = Brushes.White;
        public TimetableCell Day6 { get; set; }

        public Brush Day6Foreground { get; set; } = Brushes.Black;
        public Brush Day6Background { get; set; } = Brushes.White;
        public TimetableCell Day7 { get; set; }

        public Brush Day7Foreground { get; set; } = Brushes.Black;
        public Brush Day7Background { get; set; } = Brushes.White;

        public bool IsMerged { get; set; } // 是否合并
        public int RowSpan { get; set; }   // 行跨度
        public int ColSpan { get; set; }   // 列跨度
    }

    public class TimetableCell
    {
        public TimetableRow Row { get; set; }
        public int Column { get; set; }
        public DayOfWeek Day { get; set; } = DayOfWeek.Monday;
        public string Content { get; set; }
        public Brush Foreground { get; set; } = Brushes.Black;
        public Brush Background { get; set; } = Brushes.White;
        public bool IsMerged { get; set; } // 是否合并
        public int RowSpan { get; set; }   // 行跨度
        public int ColSpan { get; set; }   // 列跨度
    }
}
