using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.DivideintoClasses.Excels
{
    public class ImportExcelModel
    {
        public ImportExcelModel() { }

        /// <summary>
        /// 年级
        /// </summary>
        public string GradeName { get; set; } = string.Empty;
        
        /// <summary>
        /// 班级
        /// </summary>
        public string ClassName {  get; set; } = string.Empty;


        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; } = string.Empty;

        /// <summary>
        /// 课程
        /// </summary>
        public List<CourseClassHoursInfo> Courses { get; set; } =new List<CourseClassHoursInfo>();
    }
}
