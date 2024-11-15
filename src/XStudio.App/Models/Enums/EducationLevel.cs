using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.App.Models.Enums {
    public enum EducationLevel {
        
        /// <summary>
        /// 幼儿园
        /// </summary>
        [Description("幼儿园")]
        PreSchool = 0,       // 幼儿园  
        
        /// <summary>
        /// 学前班
        /// </summary>
        [Description("学前班")]
        Kindergarten,    // 学前班（在某些地区，幼儿园和学前班可能被视为同一阶段，这里为了区分而分开）  
        
        /// <summary>
        /// 小学
        /// </summary>
        [Description("54制小学")]
        PrimarySchool5,   // 小学

        /// <summary>
        /// 小学
        /// </summary>
        [Description("63制小学")]
        PrimarySchool6,   // 小学 

        /// <summary>
        /// 初中
        /// </summary>
        [Description("54制初中")]
        MiddleSchool4,    // 初中  

        /// <summary>
        /// 初中
        /// </summary>
        [Description("63制初中")]
        MiddleSchool3,    // 初中  
        
        /// <summary>
        /// 高中
        /// </summary>
        [Description("高中")]
        HighSchool,      // 高中  
        /// <summary>
        /// 大专/专科
        /// </summary>
        [Description("大专/专科")]
        JuniorCollege,   // 大专/专科（这里使用JuniorCollege来表示）
        /// <summary>
        /// 大学本科
        /// </summary>
        [Description("大学本科")]
        College,         // 大学本科（这里假设College代表本科，但在某些地区可能有不同的命名）
         /// <summary>
         /// 研究生
         /// </summary>
        [Description("研究生")]
        Graduate,        // 研究生（这里是一个泛指，包括硕士研究生和博士研究生） 
        /// <summary>
        /// 硕士研究生
        /// </summary>
        [Description("硕士研究生")]
        Masters,         // 硕士研究生（如果希望单独列出，可以添加此选项） 
        /// <summary>
        /// 博士研究生
        /// </summary>
        [Description("博士研究生")]
        PhD,             // 博士研究生（如果希望单独列出，可以添加此选项） 
        /// <summary>
        /// 成人教育/继续教育
        /// </summary>
        [Description("成人教育/继续教育")]
        AdultEducation,  // 成人教育/继续教育  
        /// <summary>
        /// 特殊教育
        /// </summary>
        [Description("特殊教育")]
        SpecialEducation, // 特殊教育
        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other,           // 其他
    }
}
