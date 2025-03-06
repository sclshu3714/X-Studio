using System.ComponentModel;

namespace XStudio.SchoolSchedule.Enums {

    /// <summary>
    /// 权重
    /// </summary>
    public enum PriorityMode {

        [Description("最高")]
        Highest = 2,

        [Description("高")]
        High = 1,

        [Description("中")]
        Medium = 0,

        [Description("低")]
        Low = -1,

        [Description("最低")]
        Lowest = -2
    }
}