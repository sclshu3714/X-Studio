using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.SchoolSchedule.Rules
{
    /// <summary>
    /// 规则接口
    /// </summary>
    public interface IRule
    {
        /// <summary>
        /// 优先级，1-N, 1 最高, N最低
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 作用类型
        /// </summary>
        public RuleMode @Mode { get; set; }
    }

    /// <summary>
    /// 作用类型
    /// </summary>
    public enum RuleMode
    {
        /// <summary>
        /// 课程
        /// </summary>
        Course,
        /// <summary>
        /// 教师
        /// </summary>
        Teacher,
    }

    /// <summary>
    /// 规则类型
    /// </summary>
    public enum RuleType
    {
        CanOnlyArrange,
        CannotBeArranged
        //Single or bi weekly
        //Liantang
        //Combined classes
        //The lesson plan is aligned
        //Courses are not adjacent
        //Disperse within the week
        //Concentration within the week
        //Centralized lesson preparation
    }
}
