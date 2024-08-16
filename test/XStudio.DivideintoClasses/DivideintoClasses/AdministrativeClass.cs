using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyConsoleApp.DivideintoClasses
{
    /// <summary>
    /// 行政班级
    /// </summary>
    public class AdministrativeClass
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }= string.Empty;
        
        /// <summary>
        /// 班级状态
        /// </summary>
        public ClassStatus Status { get; set; } = ClassStatus.EmptyClass;

        /// <summary>
        /// 班级额定人数
        /// </summary>
        public int RatedClassSize { get; set; } = 45;

        /// <summary>
        /// 班级最大人数
        /// </summary>
        public int MaxClassSize { get; set; } = 50;
    }
}
