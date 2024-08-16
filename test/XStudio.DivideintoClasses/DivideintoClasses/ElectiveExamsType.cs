using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConsoleApp.DivideintoClasses
{
    /// <summary>
    /// 物理 Physics      P
    /// 化学 Chemistry    C
    /// 生物 Biology      B
    /// 历史 History      H
    /// 政治 Politics     O
    /// 地理 Geography    G
    /// </summary>
    public enum ExamsType
    {
        /// <summary>
        /// 物理+化学+生物
        /// </summary>
        PCB = 0,
        /// <summary>
        /// 物理+化学+政治
        /// </summary>
        PCO = 1,
        /// <summary>
        /// 物理+化学+地理
        /// </summary>
        PCG = 2,
        /// <summary>
        /// 物理+生物+政治
        /// </summary>
        PBO = 3,
        /// <summary>
        /// 物理+生物+地理
        /// </summary>
        PBG = 4,
        /// <summary>
        /// 物理+政治+地理
        /// </summary>
        POG = 5,
        /// <summary>
        /// 历史+政治+地理
        /// </summary>
        HOG = 6,
        /// <summary>
        /// 历史+政治+化学
        /// </summary>
        HOC = 7,
        /// <summary>
        /// 历史+政治+生物
        /// </summary>
        HOB = 8,
        /// <summary>
        /// 历史+地理+化学
        /// </summary>
        HGC = 9,
        /// <summary>
        /// 历史+地理+生物
        /// </summary>
        HGB = 10,
        /// <summary>
        /// 历史+化学+生物
        /// </summary>
        HCB = 11
    }
}
