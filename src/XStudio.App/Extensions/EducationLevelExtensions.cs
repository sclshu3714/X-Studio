using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XStudio.App.Models.Enums;

namespace XStudio.App.Extensions {
    public static class EducationLevelExtensions {
        public static ObservableCollection<string> GetDefaultGrades(this EducationLevel level) {
            var grades = new ObservableCollection<string>();
            switch (level) {
                case EducationLevel.PreSchool:
                    grades.Add("小班");
                    grades.Add("中班");
                    grades.Add("大班");
                    break;
                case EducationLevel.Kindergarten:
                    grades.Add("学前班-衔接班");
                    break;
                case EducationLevel.PrimarySchool5:
                    grades.Add("一年级"); 
                    grades.Add("二年级"); 
                    grades.Add("三年级"); 
                    grades.Add("四年级"); 
                    grades.Add("五年级");
                    break;
                case EducationLevel.PrimarySchool6:
                    grades.Add("一年级");
                    grades.Add("二年级");
                    grades.Add("三年级");
                    grades.Add("四年级");
                    grades.Add("五年级");
                    grades.Add("六年级");
                    break;
                case EducationLevel.MiddleSchool4:
                    grades.Add("一年级");
                    grades.Add("二年级");
                    grades.Add("三年级");
                    grades.Add("四年级");
                    break;
                case EducationLevel.MiddleSchool3:
                    grades.Add("一年级");
                    grades.Add("二年级");
                    grades.Add("三年级");
                    break;
                case EducationLevel.HighSchool:
                    grades.Add("一年级");
                    grades.Add("二年级");
                    grades.Add("三年级");
                    break;
                case EducationLevel.JuniorCollege:
                    grades.Add("一年级");
                    grades.Add("二年级");
                    grades.Add("三年级");
                    grades.Add("四年级");
                    grades.Add("五年级");
                    break;
                case EducationLevel.College:
                    grades.Add("一年级");
                    grades.Add("二年级");
                    grades.Add("三年级");
                    grades.Add("四年级");
                    grades.Add("五年级");
                    break;
                case EducationLevel.Graduate:
                    grades.Add("硕士研究生");
                    grades.Add("博士研究生");
                    break;
                case EducationLevel.Masters:
                    grades.Add("硕士研究生");
                    break;
                case EducationLevel.PhD:
                    grades.Add("博士研究生");
                    break;
                case EducationLevel.AdultEducation:
                    grades.Add("一年级");
                    grades.Add("二年级");
                    grades.Add("三年级");
                    grades.Add("四年级");
                    grades.Add("五年级");
                    break;
                case EducationLevel.SpecialEducation:
                    grades.Add("一年级");
                    grades.Add("二年级");
                    grades.Add("三年级");
                    grades.Add("四年级");
                    grades.Add("五年级");
                    break;
                default:
                    grades.Add("一年级");
                    grades.Add("二年级");
                    grades.Add("三年级");
                    grades.Add("四年级");
                    grades.Add("五年级");
                    grades.Add("六年级");
                    grades.Add("七年级");
                    grades.Add("八年级");
                    grades.Add("九年级");
                    grades.Add("十年级");
                    break;
            }
            return grades;
        }
    }
}
