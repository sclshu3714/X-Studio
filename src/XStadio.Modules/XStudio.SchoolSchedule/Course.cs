using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.SchoolSchedule
{
    public class Course
    {
        /// <summary>
        /// 课程编号，主要用于快速识别和查询
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 课程名称
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
