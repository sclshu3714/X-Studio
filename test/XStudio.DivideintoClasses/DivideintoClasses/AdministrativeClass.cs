using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using XStudio.DivideintoClasses.DivideintoClasses;

namespace MyConsoleApp.DivideintoClasses
{
    /// <summary>
    /// 行政班级
    /// </summary>
    public class AdministrativeClass
    {
        /// <summary>
        /// 班级识别码
        /// </summary>
        public string Id => Guid.NewGuid().ToString();

        /// <summary>
        /// 班级编号，从1开始
        /// </summary>
        public int Number { get; set; } = 0;

        /// <summary>
        /// 名称 格式：行政1班、行政2班
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 班级状态
        /// </summary>
        public ClassStatus Status
        {
            get
            {
                if (CurrentClassSize !=0 && CurrentClassSize < MinClassSize)
                {
                    return ClassStatus.ShortageStaff;
                }
                else if (CurrentClassSize >= MinClassSize && CurrentClassSize < MaxClassSize)
                {
                    return ClassStatus.Standard;
                }
                else if (CurrentClassSize == MaxClassSize)
                {
                    return ClassStatus.FullStarffed;
                }
                else if (CurrentClassSize > MaxClassSize)
                {
                    return ClassStatus.Overstaffed;
                }
                return ClassStatus.EmptyClass;
            }
        }

        /// <summary>
        /// 班级参考人数(标准人数) (学生数量 / 班级数量) 取整数
        /// </summary>
        public int RatedClassSize { get; set; } = 45;

        /// <summary>
        /// 当前班级已经安排人数
        /// </summary>
        public int CurrentClassSize => Students == null ? 0 : Students.Count;

        /// <summary>
        /// 班级最大人数
        /// </summary>
        public int MaxClassSize { get; set; } = 50;
        /// <summary>
        /// 班级最小人数
        /// </summary>
        public int MinClassSize { get; set; } = 45;

        /// <summary>
        /// 包含学生(本班学生)
        /// </summary>
        public List<Student> Students { get; set; } = new List<Student>();

        /// <summary>
        /// 学生中的男生数量
        /// </summary>
        public int BoyNumber { get; internal set; }

        /// <summary>
        /// 学生中的女生数量
        /// </summary>
        public int GrilNummber { get; internal set; }

        /// <summary>
        /// 包含选考组合
        /// </summary>
        public List<ExaminationType> ExamsList { get; set; } = new List<ExaminationType>();

        /// <summary>
        /// 学生选考组合
        /// </summary>
        /// <returns></returns>
        public async Task<(List<StudentExamination>, MessageStruct)> StudentExamsGroup()
        {
            if (Status == ClassStatus.EmptyClass || !Students.Any())
            {
                return new(new List<StudentExamination>(),
                    new MessageStruct(CodeNumber.Warn, "还没有分班，该班为空班，不包含选考组合"));
            }
            List<StudentExamination> examGroup = Students.GroupBy(stu => stu.Exams)
                    .Select(stu => new StudentExamination() { Type = stu.Key, StudentNumber = stu.Count(), Students = stu.ToList() })
                    .ToList();
            await Task.CompletedTask;
            return new(examGroup, new MessageStruct(CodeNumber.Success));
        }
    }
}
