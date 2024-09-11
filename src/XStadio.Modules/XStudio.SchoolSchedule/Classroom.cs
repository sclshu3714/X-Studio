using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.SchoolSchedule
{
    public class Classroom
    {
        /// <summary>
        /// 教室编号
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 教室名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 教室位置(示例：学校Id&校区Id&楼栋Id&楼层Id)
        /// </summary>
        public string Position {  get; set; } = string.Empty;
    }
}
