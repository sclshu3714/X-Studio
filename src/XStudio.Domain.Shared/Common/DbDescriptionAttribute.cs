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
    public class DbDescriptionAttribute : Attribute
    {
        /// <summary>
        /// 注释内容
        /// </summary>
        public string DbDescription { get; set; }
        public DbDescriptionAttribute(string dbDescription)
        {
            DbDescription = dbDescription;
        }
    }
}
