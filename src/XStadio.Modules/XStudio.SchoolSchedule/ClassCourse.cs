using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.SchoolSchedule
{
    /// <summary>
    /// 课程
    /// </summary>
    public class ClassCourse
    {
        /// <summary>
        /// 课程编号，主要用于快速识别和查询
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 课程名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 教室Id
        /// </summary>
        public string ClassroomId { get; set; } = string.Empty;

        /// <summary>
        /// 教室名称
        /// </summary>
        public string ClassroomName { get; set; } = string.Empty;

        /// <summary>
        /// 班级Id
        /// </summary>
        public string ClassId { get; set; } = string.Empty;

        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; } = string.Empty;

        /// <summary>
        /// 教师Id
        /// </summary>
        public string TeacherId { get; set; } = string.Empty;

        /// <summary>
        /// 教师名称
        /// </summary>
        public string TeacherName { get; set; } = string.Empty;


        /// <summary>
        /// 其他教师(副老师)
        /// </summary>
        public Dictionary<string, string> OtherTeachers { get; set; } = new Dictionary<string, string>();
    }
}
