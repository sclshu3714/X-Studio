using System.ComponentModel;

namespace XStudio.SchoolSchedule.Enums {

    /// <summary>
    /// 作用类型
    /// </summary>
    public enum RuleMode {

        [Description("无")]
        None = 0,

        /// <summary>
        /// 课程
        /// </summary>
        [Description("课程")]
        Course,

        /// <summary>
        /// 教师
        /// </summary>
        [Description("教师")]
        Teacher,
    }
}