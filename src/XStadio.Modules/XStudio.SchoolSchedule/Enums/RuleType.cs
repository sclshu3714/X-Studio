using System.ComponentModel;

namespace XStudio.SchoolSchedule.Enums {

    /// <summary>
    /// 规则类型(顺序表示规则顺序)
    /// </summary>
    public enum RuleType {

        /// <summary>
        /// 只能排[课程、教师]
        /// </summary>
        [Description("只能排")]
        CanOnlyArrange = 0,

        /// <summary>
        /// 不能排[课程、教师]
        /// </summary>
        [Description("不能排")]
        CannotBeArranged = 1,

        /// <summary>
        /// 连堂[课程]
        /// </summary>
        [Description("连堂")]
        ConsecutiveClasses = 2,

        /// <summary>
        /// 单周[课程]
        /// </summary>
        [Description("单周")]
        Single = 3,

        /// <summary>
        /// 双周[课程]
        /// </summary>
        [Description("双周")]
        Biweekly = 4,

        /// <summary>
        /// 单双周[课程]
        ///     两门课交替轮询
        /// </summary>
        [Description("单双周")]
        SingleOrBiweekly = 5,

        /// <summary>
        /// 交替轮询[课程]
        ///     多门课交替轮询
        /// </summary>
        [Description("交替轮询")]
        AlternatePolling = 6,

        /// <summary>
        /// 合班[教师]
        /// </summary>
        [Description("合班")]
        JointClassTeaching = 7,

        /// <summary>
        /// 集中备课[教师]
        /// </summary>
        [Description("集中备课")]
        CentralizedLessonPreparation = 8,

        /// <summary>
        /// 互斥[教师]
        ///     两个老师不能同时上课
        /// </summary>
        [Description("互斥")]
        Mutex = 9,

        /// <summary>
        /// 同步[教师]
        ///     两个老师必须同时上课
        /// </summary>
        [Description("同步")]
        Sync = 10,

        /// <summary>
        /// 无规则
        /// </summary>
        [Description("无规则")]
        None = 11,

        /// <summary>
        /// 教案齐平[课程]
        /// </summary>
        [Description("教案齐平")]
        LessonPlanAligned = 12,

        /// <summary>
        /// 课程不相邻[课程]
        /// </summary>
        [Description("课程不相邻")]
        CoursesAreNotAdjacent = 13,

        /// <summary>
        /// 周内分散[课程、教师]
        /// </summary>
        [Description("周内分散")]
        DisperseWithinTheWeek = 14,

        /// <summary>
        /// 周内集中[教师]
        /// </summary>
        [Description("周内集中")]
        ConcentrationWithinTheWeek = 15,

        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        Unknown = 16,
    }
}