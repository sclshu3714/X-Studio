using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConsoleApp.DivideintoClasses
{
    /// <summary>
    /// 分班
    /// </summary>
    public class DivideClasses
    {
        private static Random _random = new Random();
        public static List<Student> TotalStudents;

        private static IEnumerable<Student> SimulateStudentGeneration(int studentNum)
        {
            for (int i = 0; i < studentNum; i++)
            {
                yield return new Student()
                {
                    FirstName = "A",
                    LastName = $"{i}",
                    Exams = GetRandomExamType()
                };
            }
        }
        public static ExamsType GetRandomExamType()
        {
            // 定义权重数组
            ExamsType[] weightedExams = new ExamsType[]
            {
                ExamsType.PCB, ExamsType.PCB, ExamsType.PCB, ExamsType.PCB, ExamsType.PCB,
                ExamsType.PCB, ExamsType.PCB, ExamsType.PCB, ExamsType.PCB, ExamsType.PCB,
                ExamsType.PCB, ExamsType.PCB, ExamsType.PCB, ExamsType.PCB, ExamsType.PCB,
                ExamsType.PCB, ExamsType.PCB, ExamsType.PCB, ExamsType.PCB, ExamsType.PCB,
                ExamsType.PCB, ExamsType.PCB, ExamsType.PCB, ExamsType.PCB, ExamsType.PCB,// 50%
                ExamsType.POG, ExamsType.POG, ExamsType.POG, ExamsType.POG, ExamsType.POG,
                ExamsType.POG, ExamsType.POG, ExamsType.POG, ExamsType.POG, ExamsType.POG,
                ExamsType.POG, ExamsType.POG, ExamsType.POG, ExamsType.POG, ExamsType.POG,// 30%
                ExamsType.PCO, ExamsType.PCG, ExamsType.PBO, ExamsType.PBG, ExamsType.HOG,
                ExamsType.HOC, ExamsType.HOB, ExamsType.HGC, ExamsType.HGB, ExamsType.HCB // 20%
            };

            // 随机选择一个
            int index = _random.Next(weightedExams.Length);
            return weightedExams[index];
        }

        public static bool StartDivide(int studentNum, int classNum, int minClassSize, int maxClassSize)
        {
            TotalStudents = SimulateStudentGeneration(studentNum).ToList();
            if (TotalStudents == null || !TotalStudents.Any())
            { 
                return false;
            }
            
            //定3
            

            return true;
        }

        /// <summary>
        /// 分班
        /// </summary>
        /// <param name="classNum"></param>
        /// <param name="minClassSize"></param>
        /// <param name="maxClassSize"></param>
        /// <returns></returns>
        //public List<AdministrativeClass> DistributeStudents(int classNum, int minClassSize, int maxClassSize)
        //{
        //    IEnumerable<IGrouping<ExamsType, Student>> gStudents = TotalStudents.GroupBy(stu => stu.Exams);
        //    foreach (var item in gStudents)
        //    {
        //        ExamsType exams = item.Key;
        //        List<Student> students = item.ToList();

        //    }
        //}
    }
}
