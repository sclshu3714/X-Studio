namespace XStudio.SchoolSchedule.Models {

    /// <summary>
    /// 年级
    /// </summary>
    public class Grade {

        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 包含的班级
        /// </summary>
        public List<SchoolClass> Classes { get; set; } = new List<SchoolClass>();
    }
}