using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Common
{
    /// <summary>
    /// 数据库注释扩展属性
    /// </summary>
    public class CustomDescriptionAttribute : Attribute
    {
        /// <summary>
        /// 注释内容
        /// </summary>
        public string? Description { get; set; }
        public CustomDescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}
