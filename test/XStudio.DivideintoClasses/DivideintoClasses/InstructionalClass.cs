using MyConsoleApp.DivideintoClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.DivideintoClasses.DivideintoClasses
{
    /// <summary>
    /// 教学班
    /// </summary>
    public class InstructionalClass
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
        /// 名称 格式：物理教学1班、物理教学2班
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 教学类型(物理、生物...)
        /// </summary>
        public InstructionalType Type { get; set; } = InstructionalType.Physics;

        /// <summary>
        /// 包含学生(本班学生)
        /// </summary>
        public List<Student> Students { get; set; } = new List<Student>();
    }
}
