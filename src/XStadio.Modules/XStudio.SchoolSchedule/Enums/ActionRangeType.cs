using System.ComponentModel;

namespace XStudio.SchoolSchedule.Enums {

    /// <summary>
    /// 作用范围类型
    /// </summary>
    public enum ActionRangeType {

        [Description("无")]
        None = 0,

        [Description("课程")]
        Course = 1,

        [Description("教师")]
        Teacher = 2,

        [Description("班级")]
        Class = 3,

        [Description("场所")]
        Place = 4
    }
}