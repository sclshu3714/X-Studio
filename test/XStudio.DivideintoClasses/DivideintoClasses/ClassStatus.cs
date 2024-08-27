using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConsoleApp.DivideintoClasses
{
    public enum ClassStatus
    {
        /// <summary>
        /// 空班,未开班，没有安排人员
        /// </summary>
        EmptyClass,
        /// <summary>
        /// 缺员，没有达到开班的最少人数
        /// </summary>
        ShortageStaff,
        /// <summary>
        /// 标准班，达到开班的最少人数
        /// </summary>
        Standard,
        /// <summary>
        /// 满员
        /// </summary>
        FullStarffed,
        /// <summary>
        /// 超员
        /// </summary>
        Overstaffed,
    }
}
