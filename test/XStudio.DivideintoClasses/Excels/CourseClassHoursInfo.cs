namespace XStudio.DivideintoClasses.Excels
{
    public class CourseClassHoursInfo
    {
        public CourseClassHoursInfo() { }

        /// <summary>
        /// 课程周正课时
        /// Regular class hours for the course week
        /// </summary>
        public double Regular { get; set; } = 0;

        /// <summary>
        /// 早自习课时
        /// Morning self-study class
        /// </summary>
        public double Morning { get; set; } = 0;

        /// <summary>
        /// 晚自习课时
        ///  Evening self-study class
        /// </summary>
        public double Evening { get; set; } = 0;
    }
}