using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XStudio.SchoolSchedule.Rules;

namespace XStudio.SchoolSchedule.Algorithms {

    /// <summary>
    /// 回溯算法
    /// </summary>
    public class BacktrackScheduler {

        /// <summary>
        /// 冲突记录字典
        /// </summary>
        private Dictionary<string, List<string>> Conflicts = new Dictionary<string, List<string>>();

        /// <summary>
        /// 多线程时使用的锁
        /// </summary>
        private readonly object locker = new object(); // 定义一个锁对象
        
        /// <summary>
        /// 记录没有分配的课程
        /// </summary>
        public string? NoAssignCourses { get; set; } = null;

        /// <summary>
        /// 自动分配课程
        /// </summary>
        /// <param name="classSchedule">课表节次</param>
        /// <param name="courses">需要分配的课程</param>
        /// <param name="constraint">课程约束条件</param>
        /// <returns>是否成功分配所有课程</returns>
        public bool StartAutoAssignCourses(ClassSchedule classSchedule, List<IRule> courses, List<IRule>? constraint) {
            // 清空之前的冲突记录
            Conflicts.Clear();
            NoAssignCourses = null;

            // 对课程进行排序，优先处理约束条件多的课程
            //courses = courses.OrderByDescending(c => c.GetConstraints().Count).ToList();

            // 尝试为每个课程分配时间和地点
            foreach (var course in courses) {
                bool assigned = AssignCourseToSchedule(classSchedule, course, constraint);
                
                if (!assigned) {
                    // 如果某个课程无法分配，记录未分配的课程
                    NoAssignCourses += course.DisplayName + ";";
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 为单个课程分配节次和教室
        /// </summary>
        /// <param name="classSchedule">课表</param>
        /// <param name="course">待分配的课程</param>
        /// <param name="constraints">约束条件</param>
        /// <returns>是否成功分配</returns>
        private bool AssignCourseToSchedule(ClassSchedule classSchedule, IRule course, List<IRule>? constraints) {
            // 遍历所有可能的节次和教室
            //foreach (var timeSlot in classSchedule.Sections) {
            //    foreach (var classroom in classSchedule.GetAvailableClassrooms()) {
            //        // 检查是否满足所有约束条件
            //        if (IsValidAssignment(course, timeSlot, classroom, constraints)) {
            //            // 分配课程
            //            classSchedule.AddSectionContent(course, timeSlot, classroom);
            //            return true;
            //        }
            //    }
            //}
            return false;
        }

        /// <summary>
        /// 验证课程分配是否满足约束条件
        /// </summary>
        /// <param name="course">课程</param>
        /// <param name="timeSlot">时间槽</param>
        /// <param name="classroom">教室</param>
        /// <param name="constraints">约束条件</param>
        /// <returns>是否满足所有约束</returns>
        private bool IsValidAssignment(IRule course, Classroom classroom, List<IRule>? constraints) {
            if (constraints == null) return true;
            //foreach (var constraint in constraints) {
            //    if (!constraint.CheckRule(course, timeSlot, classroom)) {
            //        // 记录冲突
            //        RecordConflict(course.DisplayName, constraint.GetType().Name);
            //        return false;
            //    }
            //}
            return true;
        }

        /// <summary>
        /// 记录课程冲突
        /// </summary>
        /// <param name="courseName">课程名称</param>
        /// <param name="conflictType">冲突类型</param>
        private void RecordConflict(string courseName, string conflictType) {
            lock (locker) {
                if (!Conflicts.ContainsKey(courseName)) {
                    Conflicts[courseName] = new List<string>();
                }
                Conflicts[courseName].Add(conflictType);
            }
        }
    }
}
