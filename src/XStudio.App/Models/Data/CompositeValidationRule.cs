using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace XStudio.App.Models.Data {
    // 验证
    public class CompositeValidationRule : ValidationRule {
        // 邮箱格式的正则表达式  
        private static readonly Regex EmailRegex = new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$");

        // 最小和最大长度（这些可以是动态的，例如通过属性或依赖项属性设置）  
        public int MinLength { get; set; } = 0;
        public int MaxLength { get; set; } = 0; // 或者你想要的任何默认值  

        public ValidationRuleType @Type { get; set; } = ValidationRuleType.NoEmpty;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
            string? input = value as string;
            switch (@Type) {
                case ValidationRuleType.NoEmpty:
                    if (string.IsNullOrEmpty(input)) {
                        return new ValidationResult(false, "输入不能为空。");
                    }
                    break;
                case ValidationRuleType.UserName:
                    if (string.IsNullOrEmpty(input)) {
                        return new ValidationResult(false, "输入不能为空。");
                    }
                    // 验证邮箱格式  
                    if (input.Contains("@") && !EmailRegex.IsMatch(input)) {
                        return new ValidationResult(false, "邮箱格式不正确。");
                    }

                    // 验证输入长度范围  
                    if (MinLength >= 0 && MaxLength > 0 && MaxLength >= MinLength && (input.Length < MinLength || input.Length > MaxLength)) {
                        return new ValidationResult(false, $"输入长度必须在 {MinLength} 到 {MaxLength} 个字符之间。");
                    }
                    break;
                case ValidationRuleType.Password:
                    if (string.IsNullOrEmpty(input)) {
                        return new ValidationResult(false, "输入不能为空。");
                    }
                    // 验证输入长度范围  
                    if (MinLength >= 0 && MaxLength > 0 && MaxLength >= MinLength && (input.Length < MinLength || input.Length > MaxLength)) {
                        return new ValidationResult(false, $"输入长度必须在 {MinLength} 到 {MaxLength} 个字符之间。");
                    }
                    break;
                default:
                    break;
            }
            // 如果所有验证都通过，则返回 true 和空消息  
            return new ValidationResult(true, null);
        }
    }
}
