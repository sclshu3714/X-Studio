using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.SchoolSchedule {
    /// <summary>
    /// 调度算法类型
    /// </summary>
    public enum SchedulerType {
        /// <summary>
        /// 贪心算法
        /// </summary>
        Greedy,
        /// <summary>
        /// 回溯算法
        /// </summary>
        Backtrack,
        /// <summary>
        /// 遗传算法
        /// </summary>
        Genetic
    }
}
