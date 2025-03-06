using XStudio.SchoolSchedule.Constraints;
using XStudio.SchoolSchedule.Rules;

namespace XStudio.SchoolSchedule.Algorithms {

    /// <summary>
    /// 基于机器学习的课程调度算法
    /// </summary>
    public class MLCourseScheduler {

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
        /// <param name="classSchedule">课表节次即课程安排节次内容</param>
        /// <param name="courses">需要分配的课程</param>
        /// <param name="constraint">课程约束条件</param>
        /// <returns>是否成功分配</returns>
        public bool StartAutoAssignCourses(ClassSchedule classSchedule, List<IRule> courses, List<IConstraint>? constraint) {
            // 清空之前的冲突记录
            NoAssignCourses = null;
            // TODO: 使用ML.Net实现自动排课功能

            return true;
        }
    }
}