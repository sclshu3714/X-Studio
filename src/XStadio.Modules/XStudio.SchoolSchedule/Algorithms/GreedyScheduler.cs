using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XStudio.SchoolSchedule.Rules;

namespace XStudio.SchoolSchedule.Algorithms {

    /// <summary>
    /// 贪心算法
    /// </summary>
    public class GreedyScheduler {

        // 冲突记录字典
        private Dictionary<string, List<string>> Conflicts;

        // 多线程时使用的锁
        private readonly object locker = new object();

        public GreedyScheduler() {
            Conflicts = new Dictionary<string, List<string>>();
        }


        /// <summary>
        /// 记录没有分配的课程
        /// </summary>
        public string? NoAssignCourses { get; set; } = null;

        /// <summary>
        /// 自动分配课程的函数
        /// </summary>
        /// <param name="classSchedule">课表节次</param>
        /// <param name="courses">规则课程</param>
        /// <param name="constraint">约束</param>
        /// <param name="index">已分配课程的索引</param>
        /// <returns></returns>
        /// <example>
        /// 注意：courses 是记录了所有课时课程的集合，有顺序基础
        /// 1. 优先完成确定坐标的课，
        /// 如：作用类型是课程的只能排
        ///     作用类型是老师的只能排、互斥与同步
        /// 互斥与同步: 未指定时间的，两个老师在整个周期内互斥或者同步；   指定时间坐标的，两个老师在指定时间坐标互斥或者同步(指定位置不一定有这两个老师的课)。
        /// </example>
        public bool StartAutoAssignCourses(ClassSchedule classSchedule, List<IRule> courses, List<IRule>? constraint) {
            NoAssignCourses = null;
            if(constraint != null && constraint.Count > 0) {
                // 先检查约束, 看看是否存在只能排，如果存在只能排，则优先排只能排的课程
                classSchedule.RunCanOnlyArrange(courses, constraint.Where(x => x.Type == RuleType.CanOnlyArrange));
                constraint = constraint.Where(x => x.Type != RuleType.CanOnlyArrange && x.Type != RuleType.ConsecutiveClasses).ToList();
            }

            return AutoAssignCourses(classSchedule, courses, constraint);
        }

        /// <summary>
        /// 自动分配课程的函数
        /// </summary>
        /// <param name="classSchedule">课表节次</param>
        /// <param name="courses">规则课程</param>
        /// <param name="constraint">约束</param>
        /// <param name="index">已分配课程的索引</param>
        /// <returns></returns>
        private bool AutoAssignCourses(ClassSchedule classSchedule,
                                        List<IRule> courses,
                                        List<IRule>? constraint) {
            var noAssignCoursesList = new List<string>(); // 记录无法分配的课程
            foreach(IRule rule in courses) {
                // 获取可用节次
                Section? section = classSchedule.GetAvailableSections(rule, rule.RestrictType);
                // 验证是否可以分配到该节次
                Tuple<bool, string> tupleAssign = classSchedule.CanAssign(section, rule, constraint);
                if(tupleAssign.Item1 && section != null) {
                    // 该课可以分配，分配课程
                    doAutoAssignCourses(classSchedule, rule, section);
                    continue;
                }
                // 该课无法分配，添加到无法分配的课程列表
                noAssignCoursesList.Add(rule.DisplayName);
            };
            NoAssignCourses = string.Join(",", noAssignCoursesList);
            return !noAssignCoursesList.Any(); // 该课程无法分配，返回失败 
        }

        /// <summary>
        /// 安排课程
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="section"></param>
        private void doAutoAssignCourses(ClassSchedule classSchedule, IRule rule, Section section) {
            switch(rule.Type) {
                case RuleType.ConsecutiveClasses: // 连堂课
                    doAssignConsecutiveClassesCourses(classSchedule, rule, section);
                    break;
                case RuleType.AlternatePolling: // 交替轮换课
                    doAssignAlternatePollingCourses(classSchedule, rule, section);
                    break;
                case RuleType.SingleOrBiweekly: // 单双周课
                    classSchedule.AddSectionContent(section.Code, new SectionContent(0, rule, 1));
                    break;
                default:
                    classSchedule.AddSectionContent(section.Code, new SectionContent(0, rule));
                    break;
            }
        }

        /// <summary>
        /// 安排交替轮换课
        /// </summary>
        /// <param name="classSchedule"></param>
        /// <param name="rule"></param>
        /// <param name="section"></param>
        private void doAssignAlternatePollingCourses(ClassSchedule classSchedule, IRule rule, Section section) {
            AlternatePolling polling = (AlternatePolling)rule;
            classSchedule.AddSectionContent(section.Code, new SectionContent(0, polling, polling.PollingCourses.Count - 1));
        }

        /// <summary>
        /// 安排连堂课
        /// </summary>
        /// <param name="classSchedule"></param>
        /// <param name="rule"></param>
        /// <param name="section"></param>
        private void doAssignConsecutiveClassesCourses(ClassSchedule classSchedule, IRule rule, Section section) {
            ConsecutiveClasses continuousClasses = (ConsecutiveClasses)rule;
            continuousClasses.Periods = new List<int>() { section.Period, section.Period + 1 };
            classSchedule.AddSectionContent(section.Code, new SectionContent(0, continuousClasses));
            section = classSchedule[section.Day, section.Period + 1];
            classSchedule.AddSectionContent(section.Code, new SectionContent(0, continuousClasses));
        }
    }
}
