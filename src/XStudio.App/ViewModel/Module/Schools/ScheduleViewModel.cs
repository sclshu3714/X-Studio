using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XStudio.App.Extensions;
using XStudio.App.Models.Enums;
using XStudio.App.Service;

namespace XStudio.App.ViewModel.Module.Schools {
    public class ScheduleViewModel : ViewModelBase {
        private int _order = 0;
        private string _code = string.Empty;
        private string _name = string.Empty;
        private EducationLevel _period = EducationLevel.Other;
        private DateTime _startDate = DateTime.Now;
        private DateTime _endDate = DateTime.Now.AddMonths(6);
        private string _grade = string.Empty;
        private bool _isSelected = false;
        private string _schoolYear = string.Empty;
        private ObservableCollection<string> _defaultGrades = new ObservableCollection<string>();
        private string _semester = string.Empty;

        [Description("序号")]
        public int Order {
            get => _order;
            set => SetProperty(ref _order, value);
        }
        /// <summary>
        /// 节次表编号，主要用于快速识别和查询
        /// </summary>
        [Description("编号")]
        public string Code {
            get => _code;
            set => SetProperty(ref _code, value);
        }

        /// <summary>
        /// 节次表名称
        /// </summary>
        [Description("节次方案名称")]
        public string Name {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// 学段(幼儿园、学前班、小学、初中、高中、大学、研究生、博士生、其他)
        /// </summary>
        [Description("学段")]
        public EducationLevel Period {
            get => _period;
            set {
                if (SetProperty(ref _period, value)) {
                    DefaultGrades = value.GetDefaultGrades();
                }
            }
        }


        /// <summary>
        /// 可选年级,依赖学段Period的值
        ///     幼儿园（大班、小班）；小学（一年级、二年级、三年级、四年级,五年级）；初中（六年级、七年级、八年级、九年级）；高中（高一、高二、高三）；大学（大一、大二、大三、大四）；研究生（硕士研究生、博士研究生）；博士生（博士一、博士二、博士三）；其他
        /// </summary>
        [Description("可选年级")]
        public ObservableCollection<string> DefaultGrades {
            get => _defaultGrades;
            set => SetProperty(ref _defaultGrades, value);
        }


        /// <summary>
        /// 年级 - 幼儿园（大班、小班）；小学（一年级、二年级、三年级、四年级,五年级）；初中（六年级、七年级、八年级、九年级）；高中（高一、高二、高三）；大学（大一、大二、大三、大四）；研究生（硕士研究生、博士研究生）；博士生（博士一、博士二、博士三）；其他
        /// </summary>
        [Description("年级")]
        public string Grade {
            get => _grade;
            set => SetProperty(ref _grade, value);
        }

        /// <summary>
        /// 学年(2024-2025)
        /// </summary>
        [Description("学年")]
        public string SchoolYear {
            get => _schoolYear;
            set => SetProperty(ref _schoolYear, value);
        }

        /// <summary>
        /// 学期开始日期
        /// </summary>
        [Description("学期开始日期")]
        public DateTime StartDate {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }


        /// <summary>
        /// 学期结束日期
        /// </summary>
        [Description("学期结束日期")]
        public DateTime EndDate {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }

        /// <summary>
        /// 学期(上学期/下学期、第一学期/第二学期/第三学期)
        /// </summary>
        [Description("学期")]
        public string Semester { 
            get=>_semester;
            set=>SetProperty(ref _semester, value);
        }


        /// <summary>
        /// 默认周一 - 周日,当然也可以控制周日 - 周六,周六 - 周五等
        /// </summary>
        [Description("布局节次表")]
        public ObservableCollection<DayOfWeek> LayoutOfWeek { get; set; } = new ObservableCollection<DayOfWeek>() {
             DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
             DayOfWeek.Friday, DayOfWeek.Saturday,DayOfWeek.Sunday
        };

        [Description("是否选中")]
        public bool IsSelected {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
    }
}
