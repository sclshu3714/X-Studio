using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Schools.Timetable.Attributes
{
    /// <summary>
    /// 自习校验
    /// </summary>
    public class SelfStudyIfTeachingAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var section = (Section)validationContext.ObjectInstance;

            if (!section.IsTeaching && section.IsSelfStudy)
            {
                return new ValidationResult("自习需要在授课模式下才能启用");
            }

            return ValidationResult.Success;
        }
    }
}
