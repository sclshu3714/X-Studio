using Prism.Mvvm;
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
    public class TimetableRow : BindableBase {
        private string timeSlot;
        private int period;
        private ObservableCollection<TimetableCell> cells = new();
        private TimetableCell monday;
        private TimetableCell tuesday;
        private TimetableCell webnesday;
        private TimetableCell thursday;
        private TimetableCell saturday;
        private TimetableCell friday;
        private TimetableCell sunday;

        /// <summary>
        /// 时段
        /// </summary>
        public string TimeSlot  {
            get => timeSlot;
            set => SetProperty(ref timeSlot, value);
        }

        /// <summary>
        /// 节次
        /// </summary>
        public int Period { 
            get => period;
            set => SetProperty(ref period, value);
        }
        /// <summary>
        /// 节次名称
        /// </summary>
        public string PeriodName => $"第 {Period} 节";

        /// <summary>
        /// 单元格集合
        /// </summary>
        public ObservableCollection<TimetableCell> Cells { 
            get => cells;
            set => SetProperty(ref cells, value);
        }

        /// <summary>
        /// 周一至周日的单元格
        /// </summary>
        public TimetableCell Monday {
            get => monday;
            set => SetProperty(ref monday, value);
        }

        public TimetableCell Tuesday {
            get => tuesday;
            set => SetProperty(ref tuesday, value);
        }

        public TimetableCell Wednesday {
            get=>webnesday;
            set=>SetProperty(ref webnesday, value);
        }

        public TimetableCell Thursday { 
            get=>thursday;
            set=>SetProperty(ref thursday, value);
        }

        public TimetableCell Friday {
            get => friday;
            set => SetProperty(ref friday, value);
        }

        public TimetableCell Saturday {
            get => saturday;
            set => SetProperty(ref saturday, value);
        }

        public TimetableCell Sunday {
            get => sunday;
            set => SetProperty(ref sunday, value);
        }
    }

    public class TimetableCell : BindableBase {
        private TimetableRow row;
        private int column;
        private DayOfWeek day = DayOfWeek.Monday;
        private string content;
        private Brush foreground = Brushes.Black;
        private Brush background = Brushes.White;
        private bool merged = false;
        private int rowSpan = 1;
        private int colSpan = 1;

        public TimetableRow Row { 
            get => row;
            set => SetProperty(ref row, value);
        }
        
        public int Column { 
            get => column;
            set => SetProperty(ref column, value);
        }
        public DayOfWeek Day {
            get => day;
            set => SetProperty(ref day, value);
        }
        public string Content { 
            get => content;
            set => SetProperty(ref content, value);
        }
        public Brush Foreground { 
            get =>foreground; 
            set => SetProperty(ref foreground, value);
        }
        public Brush Background {
            get => background;
            set => SetProperty(ref background, value);
        }
        public bool IsMerged {
            get =>merged;
            set => SetProperty(ref merged, value);
        }
        public int RowSpan { 
            get => rowSpan;
            set => SetProperty(ref rowSpan, value);
        }
        public int ColSpan {
            get => colSpan;
            set => SetProperty(ref colSpan, value);
        }
    }
}
