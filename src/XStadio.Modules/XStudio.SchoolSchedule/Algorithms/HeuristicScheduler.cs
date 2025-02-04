using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XStudio.SchoolSchedule.Rules;

namespace XStudio.SchoolSchedule.Algorithms {

    /// <summary>
    /// 启发式算法
    /// </summary>
    public class HeuristicScheduler {
        // 锁对象
        private readonly object locker = new object();

        public HeuristicScheduler() {
            Conflicts = new Dictionary<string, List<string>>();
        }

        /// <summary>
        /// 未分配课程
        /// </summary>
        public string? NoAssignCourses { get; set; } = null;

        /// <summary>
        /// 记录冲突
        /// </summary>
        private Dictionary<string, List<string>> Conflicts { get; set; }

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
        public bool StartAutoAssignCourses(SchedulerType schedulerType, ClassSchedule classSchedule, List<IRule> courses, List<IRule>? constraint) {
            switch (schedulerType) {
                case SchedulerType.Greedy:
                    return StartGreedyAssignCourses(classSchedule, courses, constraint);
                case SchedulerType.Backtrack:
                    return StartBacktrackingAssignCourses(classSchedule, courses, constraint);
                    case SchedulerType.Genetic:
                    return StartGeneticAssignCourses(classSchedule, courses, constraint);
                default:
                    return false;
            }
        }

        /// <summary>
        /// 使用遗传算法自动分配课程
        /// </summary>
        /// <param name="classSchedule"></param>
        /// <param name="courses"></param>
        /// <param name="constraint"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private bool StartGeneticAssignCourses(ClassSchedule classSchedule, List<IRule> courses, List<IRule>? constraint) {
            GeneticScheduler geneticScheduler = new GeneticScheduler();
            if (geneticScheduler.StartAutoAssignCourses(classSchedule, courses, constraint)) {
                return true;
            }
            NoAssignCourses = string.Join(",", geneticScheduler.NoAssignCourses);
            return false;
        }

        /// <summary>
        /// 使用回溯算法自动分配课程
        /// </summary>
        /// <param name="classSchedule"></param>
        /// <param name="courses"></param>
        /// <param name="constraint"></param>
        /// <returns></returns>
        private bool StartBacktrackingAssignCourses(ClassSchedule classSchedule, List<IRule> courses, List<IRule>? constraint) {
            return false;
        }

        /// <summary>
        /// 使用贪心算法自动分配课程
        /// </summary>
        /// <param name="classSchedule"></param>
        /// <param name="courses"></param>
        /// <param name="constraint"></param>
        /// <returns></returns>
        private bool StartGreedyAssignCourses(ClassSchedule classSchedule, List<IRule> courses, List<IRule>? constraint) {
            GreedyScheduler greedyScheduler = new GreedyScheduler();
            if(greedyScheduler.StartAutoAssignCourses(classSchedule, courses, constraint)) {
                return true;
            }
            NoAssignCourses = greedyScheduler.NoAssignCourses;
            return false;
        }
    }
}
