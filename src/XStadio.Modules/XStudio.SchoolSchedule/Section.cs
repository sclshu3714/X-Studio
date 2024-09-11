using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.SchoolSchedule
{
    /// <summary>
    /// 课程表中的节次
    /// </summary>
    /// <example>
    /// 第一周星期一上午第二节
    /// </example>
    public class Section
    {
        /// <summary>
        /// 教学周(从第1周开始计数)
        /// </summary>
        public int TeachingWeek { get; set; } = 1;
        /// <summary>
        /// 星期
        /// </summary>
        public DayOfWeek Week { get; set; } = DayOfWeek.Monday;

        /// <summary>
        /// 时段
        /// </summary>
        public string TimePeriod { get; set; } = "上午";

        /// <summary>
        /// 第几节(从1开始计数)
        /// </summary>
        public int Index { get; set; } = 1;

        /// <summary>
        /// 节次名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 内容
        /// </summary>
        public List<SectionContent> Contents { get; set; } = new List<SectionContent>();

    }

    /// <summary>
    /// 节次内容
    /// </summary>
    public class SectionContent
    { 
        /// <summary>
        /// 序列号，控制显示顺序
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 课程Id
        /// </summary>
        public string CourseId { get; set; } = string.Empty;

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; } = string.Empty;

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

        /// <summary>
        /// 每周间隔
        /// </summary>
        /// <example>
        /// 常规课：每周都需要上         => 0
        /// 单双周：单周或者双周才上的课 => 单周 1 | 双周 2
        /// 轮巡周：每间隔多少周轮巡1次  => 轮巡次数 n， n <= 学期周 最少一个学期上1次， 如设置为3，那么每3周上一次课，该节次内容可以排3个课程，轮巡上课
        /// </example>
        public int WeeklyInterval { get; set; } = 0;
    }
}
