using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.SchoolSchedule {

    /// <summary>
    /// 年级课程
    /// </summary>
    public class ClassCourse {
        public ClassCourse (string code, string name, float classHour) {
            Code = code;
            Name = name;
            ClassHour = classHour;
        }

        public string Code { get; set; }
        public string Name { get; set; }

        public float ClassHour { get; set; }

    }
}
