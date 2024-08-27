using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.DivideintoClasses.DivideintoClasses
{
    public enum InstructionalClassType
    {

    }

    /// <summary>
    /// 教学课程类型
    /// </summary>
    public enum InstructionalType
    {
        [Description("物理")]
        Physics,
        [Description("化学")]
        Chemistry,
        [Description("生物")]
        Biology,
        [Description("历史")]
        History,
        [Description("政治")]
        Politics,
        [Description("地理")]
        Geography,
    }
}
