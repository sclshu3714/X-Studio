using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.App.Models.Data
{
    public enum ValidationRuleType
    {
        /// <summary>
        /// 验证输入不为空
        /// </summary>
        NoEmpty,

        /// <summary>
        /// 用户名输入验证
        ///     用户名可以时名称或者邮箱，不能为空
        ///     用户名长度位6-16
        /// </summary>
        UserName,

        /// <summary>
        /// 登录界面密码验证
        ///     输入密码不能为空
        /// </summary>
        Password,
    }
}
