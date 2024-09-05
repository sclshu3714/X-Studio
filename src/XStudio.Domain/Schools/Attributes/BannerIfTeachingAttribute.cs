using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Schools.Timetable.Attributes
{
    /// <summary>
    /// 通栏校验
    /// </summary>
    public class BannerIfTeachingAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var section = (Section)validationContext.ObjectInstance;

            if (section.IsTeaching && section.IsBanner)
            {
                return new ValidationResult("通栏需要在非授课模式下才能启用");
            }

            return ValidationResult.Success;
        }
    }
}
