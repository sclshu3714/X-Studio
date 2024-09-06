using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Attributes
{
    /// <summary>
    /// 验证数据
    /// </summary>
    /// <example>
    /// 当启用文件服务器时,appToken与accessToken不能为空
    /// public bool fileServerEnable { get; set; } = false;
    /// [RequiredIfFileServerEnabled("fileServerEnable", ErrorMessage = "appToken不能为空")]
    /// public string appToken { get; set; }
    /// [RequiredIfFileServerEnabled("fileServerEnable", ErrorMessage = "accessToken不能为空")]
    ///public string accessToken { get; set; }
    /// </example>
    public class RequiredIfPropertyEnabledAttribute : ValidationAttribute
    {
        private readonly string _propertyName;
        public RequiredIfPropertyEnabledAttribute(string propertyName)
        {
            _propertyName = propertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_propertyName);
            if (property == null)
            {
                return new ValidationResult($"未知属性: {_propertyName}");
            }
            var getValue = property.GetValue(validationContext.ObjectInstance);
            bool propertyValue = getValue != null && (bool)getValue;
            if (propertyValue && string.IsNullOrEmpty(value as string))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
