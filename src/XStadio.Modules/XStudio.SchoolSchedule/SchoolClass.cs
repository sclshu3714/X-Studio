using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.SchoolSchedule {
    /// <summary>
    /// 班级
    /// </summary>
    public class SchoolClass {
        /// <summary>
        /// 班级ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 班级名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 教师ID
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// 教师姓名
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// 教室ID
        /// </summary>
        public string ClassroomId { get; set; }
    }
}
