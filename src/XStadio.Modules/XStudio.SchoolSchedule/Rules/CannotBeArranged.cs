﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.SchoolSchedule.Rules {
    public class CannotBeArranged : IRule {
        public CannotBeArranged(PriorityMode priority, RuleMode mode, ClassCourseRule classCourse, Tuple<DayOfWeek, int> location)
            : base(priority, mode, RuleType.CannotBeArranged) {
            Location = location;
            ClassCourse = classCourse;
        }

        /// <summary>
        /// 显示名称
        /// </summary>
        public override string DisplayName {
            get {
                switch (Mode) {
                    case RuleMode.Course:
                        return $"{ClassCourse.DisplayName}";
                    case RuleMode.Teacher:
                        return $"{ClassCourse.TeacherName}";
                    default:
                        break;
                }
                return "无";
            }
        }

        /// <summary>
        /// 课程和老师信息
        /// </summary>
        public ClassCourseRule ClassCourse { get; set; }
    }
}