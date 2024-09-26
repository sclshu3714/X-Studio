using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Common
{
    /// <summary>
    /// 数据状态(A、E、 S、 D)
    /// </summary>
    public enum ValidStateType
    {
        /// <summary>
        /// 正常数据 Normal, 简写A
        /// </summary>
        [Description("正常数据, 简写A")]
        A,

        /// <summary>
        /// 异常数据Abnormal, 简写E
        /// </summary>
        [Description("异常数据, 简写E")]
        E,

        /// <summary>
        /// 停用数据Deactivated, 简写S
        /// </summary>
        [Description("停用数据, 简写S")]
        S,

        /// <summary>
        /// 删除数据Deactivated, 简写D
        /// </summary>
        [Description("删除数据, 简写D")]
        D
    }
}
