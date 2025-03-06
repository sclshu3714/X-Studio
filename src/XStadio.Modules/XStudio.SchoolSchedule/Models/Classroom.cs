namespace XStudio.SchoolSchedule.Models {

    /// <summary>
    /// 教室
    /// </summary>
    public class Classroom {

        /// <summary>
        /// 教室编号
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 教室名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 教室位置
        /// (示例：学校Id/校区Id/楼栋Id/楼层Id)
        /// </summary>
        public string Position { get; set; } = string.Empty;

        /// <summary>
        /// 教室位置描述(示例：学校名称/校区名称/楼栋名称/楼层名称)
        /// </summary>
        public string PositionDescription { get; set; } = string.Empty;

        /// <summary>
        /// 教室容量(可以容纳多少学生)
        /// </summary>
        public int Accommodate { get; set; } = 45;

        /// <summary>
        /// 教室类型(如：实验室、教室、会议室)
        /// </summary>
        public string RoomType { get; set; } = string.Empty;

        /// <summary>
        /// 教室状态(如：空闲、使用中、维修中)
        /// </summary>
        public string Status { get; set; } = string.Empty;
    }
}