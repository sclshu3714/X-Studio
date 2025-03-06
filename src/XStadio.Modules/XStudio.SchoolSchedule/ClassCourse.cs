namespace XStudio.SchoolSchedule {

    /// <summary>
    /// 课程
    /// </summary>
    public class ClassCourse {

        public ClassCourse(string code, string name, float classHour) {
            Code = code;
            Name = name;
            ClassHour = classHour;
        }

        public string Code { get; set; }
        public string Name { get; set; }

        public float ClassHour { get; set; }
    }
}