using MyConsoleApp.DivideintoClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.DivideintoClasses.DivideintoClasses
{
    public class StudentExamination
    {
        public ExaminationType Type { get; set; } = ExaminationType.PCO;
        public int StudentNumber { get; set; } = 0;
        public List<Student> Students { get; set; } = new List<Student>();
    }
}
