using Newtonsoft.Json;
using Serilog;
using System.Collections.Concurrent;
using System.Data;
using System.Diagnostics;
using XStudio.SchoolSchedule.Enums;
using XStudio.SchoolSchedule.Rules;

namespace XStudio.SchoolSchedule.Algorithms {

    /// <summary>
    /// 遗传算法
    /// </summary>
    public class GeneticScheduler {

        // 种群大小
        private const int PopulationSize = 10;

        // 最大迭代次数
        private const int MaxGenerations = 50;

        // 交叉率
        private const double CrossoverRate = 0.8;

        // 变异率
        private const double MutationRate = 0.1;

        /// <summary>
        /// 多线程时使用的锁
        /// </summary>
        private readonly object locker = new object(); // 定义一个锁对象

        /// <summary>
        /// 记录没有分配的课程
        /// </summary>
        public List<string> NoAssignCourses { get; set; } = new List<string>();

        /// <summary>
        /// 冲突记录字典
        ///     key:   节次ID
        ///     value: 冲突内容
        /// </summary>
        public Dictionary<string, List<Conflict>> Conflicts { get; set; } = new Dictionary<string, List<Conflict>>();

        private JsonSerializerSettings jsonSettings = new JsonSerializerSettings {
            TypeNameHandling = TypeNameHandling.All
        };

        /// <summary>
        /// 自动分配课程
        /// </summary>
        /// <param name="classSchedule">课表节次</param>
        /// <param name="courses">需要分配的课程</param>
        /// <param name="constraint">课程约束条件</param>
        /// <returns>是否成功分配</returns>
        public bool StartAutoAssignCourses(ClassSchedule classSchedule, List<IRule> courses, List<IRule> constraint) {
            if(courses == null || courses.Count == 0)
                return false;
            constraint.ForEach(rule => {
                if(rule != null && rule.Location != null) {
                    classSchedule[rule.Location.Item1, rule.Location.Item2]?.AddSectionConstraint(rule);
                }
            });
            // 开始启动计算代码执行时间
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            // 初始化种群
            var population = InitializePopulation(classSchedule, courses, constraint);
            stopwatch.Stop();
            Log.Information($"初始化种群耗时：{stopwatch.ElapsedMilliseconds / 1000.0} s");
            stopwatch.Restart();
            for(int generation = 0; generation < MaxGenerations; generation++) {
                // 评估适应度
                var evaluatedPopulation = population.Select(schedule => new EvaluatedSchedule {
                    Schedule = schedule,
                    Fitness = EvaluateFitness(schedule, courses, constraint)
                }).OrderByDescending(x => x.Fitness).ToList();

                // 检查是否找到最优解
                if(evaluatedPopulation.First().Fitness == 1.0) {
                    // 找到完美解决方案
                    ApplyBestSchedule(classSchedule, evaluatedPopulation.First().Schedule);
                    stopwatch.Stop();
                    Log.Information($"遗传算法耗时：{stopwatch.ElapsedMilliseconds / 1000.0} s");
                    return true;
                }

                // 选择
                population = Selection(evaluatedPopulation);

                // 交叉
                population = Crossover(population);

                // 变异
                population = Mutation(population, classSchedule, courses, constraint);
            }
            // 如果达到最大迭代次数，选择最优解
            var finalEvaluatedPopulation = population.Select(schedule => new {
                Schedule = schedule,
                Fitness = EvaluateFitness(schedule, courses, constraint)
            }).OrderByDescending(x => x.Fitness).ToList();

            ApplyBestSchedule(classSchedule, finalEvaluatedPopulation.First().Schedule);
            stopwatch.Stop();
            Log.Information($"遗传算法耗时：{stopwatch.ElapsedMilliseconds / 1000.0} s");
            return true;
        }

        /// <summary>
        /// 初始化种群
        /// </summary>
        private List<ClassSchedule> InitializePopulation(ClassSchedule baseSchedule, List<IRule> courses, List<IRule> constraint) {
            var population = new ConcurrentQueue<ClassSchedule>();
            var random = new Random();
            Parallel.For(0, PopulationSize, i => {
                var schedule = JsonConvert.DeserializeObject<ClassSchedule>(JsonConvert.SerializeObject(baseSchedule, jsonSettings), jsonSettings);
                if(schedule == null)
                    return;
                // 创建课程的副本，避免修改原始列表
                var coursesQueue = new ConcurrentQueue<IRule>(courses);
                var remainingCourses = new ConcurrentQueue<IRule>();
                while(coursesQueue.Any()) {
                    if(coursesQueue.TryDequeue(out IRule? course) && course != null) {
                        var availableSection = schedule.GetAvailableSections(course, course.RestrictType, constraint);
                        if(availableSection == null) {
                            remainingCourses.Append(course);
                            continue;
                        }
                        doAutoAssignCourses(schedule, course, availableSection);
                    }
                }
                if(remainingCourses.Any()) {
                    // 未分配的课程, 将课程安排到冲突最少的节次
                    //foreach(var course in remainingCourses) {
                    //    var conflictedSection = schedule.GetAvailableSections(course, SectionType.None);
                    //    if(conflictedSection == null) {
                    //        NoAssignCourses.Add(course.DisplayName);
                    //        continue;
                    //    }
                    //    doAutoAssignCourses(schedule, course, conflictedSection);
                    //}
                }
                population.Enqueue(schedule);
            });

            return population.ToList();
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
        private bool doAssignConsecutiveClassesCourses(ClassSchedule classSchedule, IRule rule, Section? section) {
            if(section == null)
                return false;
            ConsecutiveClasses continuousClasses = (ConsecutiveClasses)rule;
            continuousClasses.Periods = new List<int>() { section.Period, section.Period + 1 };
            classSchedule.AddSectionContent(section.Code, new SectionContent(0, continuousClasses));
            section = classSchedule[section.Day, section.Period + 1];
            if(section != null) {
                classSchedule.AddSectionContent(section.Code, new SectionContent(0, continuousClasses));
            }
            return true;
        }

        /// <summary>
        /// 评估适应度
        /// </summary>
        private double EvaluateFitness(ClassSchedule schedule, List<IRule> courses, List<IRule>? constraint) {
            int totalCourses = courses.Count;
            int placedCourses = 0;

            foreach(var course in courses) {
                var section = schedule.Sections.FirstOrDefault(s => s.Contents.Any(c => c.Content?.Id == course.Id));
                Tuple<bool, string> tuple = schedule.HasCourseConflict(section, course, constraint);
                if(!tuple.Item1) {
                    placedCourses++;
                }
            }

            return (double)placedCourses / totalCourses;
        }

        /// <summary>
        /// 选择
        /// </summary>
        private List<ClassSchedule> Selection(List<EvaluatedSchedule> evaluatedPopulation) {
            var selectedPopulation = new List<ClassSchedule>();
            var random = new Random();

            // 轮盘赌选择
            // 这里的选择策略是：
            // 1. 选择适应度最高的20%
            // 2. 随机排序
            // 3. 选择第一个
            // 4. 重复以上步骤，直到选出种群大小个个体
            for(int i = 0; i < PopulationSize; i++) {
                var selectedSchedule = evaluatedPopulation
                    .OrderByDescending(x => x.Fitness)
                    .Take((int)(PopulationSize * 0.2))
                    .OrderBy(x => random.Next())
                    .First().Schedule;

                selectedPopulation.Add(selectedSchedule);
            }

            return selectedPopulation;
        }

        /// <summary>
        /// 交叉
        /// </summary>
        private List<ClassSchedule> Crossover(List<ClassSchedule> population) {
            var newPopulation = new List<ClassSchedule>();
            var random = new Random();

            for(int i = 0; i < population.Count; i += 2) {
                if(i + 1 >= population.Count)
                    break;

                if(random.NextDouble() < CrossoverRate) {
                    var parent1 = population[i];
                    var parent2 = population[i + 1];

                    var child1 = CrossoverSchedules(parent1, parent2);
                    var child2 = CrossoverSchedules(parent2, parent1);

                    newPopulation.Add(child1);
                    newPopulation.Add(child2);
                }
                else {
                    newPopulation.Add(population[i]);
                    newPopulation.Add(population[i + 1]);
                }
            }

            return newPopulation;
        }

        /// <summary>
        /// 变异
        /// </summary>
        private List<ClassSchedule> Mutation(List<ClassSchedule> population, ClassSchedule baseSchedule, List<IRule> courses, List<IRule>? constraint) {
            var random = new Random();

            for(int i = 0; i < population.Count; i++) {
                if(random.NextDouble() < MutationRate) {
                    var mutatedSchedule = MutateSchedule(population[i], baseSchedule, courses, constraint);
                    population[i] = mutatedSchedule;
                }
            }

            return population;
        }

        /// <summary>
        /// 应用最佳课表
        /// </summary>
        private void ApplyBestSchedule(ClassSchedule targetSchedule, ClassSchedule bestSchedule) {
            targetSchedule.Sections = bestSchedule.Sections;
        }

        /// <summary>
        /// 交叉两个课表
        /// </summary>
        /// <param name="parent1">父代课表1</param>
        /// <param name="parent2">父代课表2</param>
        /// <returns>子代课表</returns>
        private ClassSchedule CrossoverSchedules(ClassSchedule parent1, ClassSchedule parent2) {
            // 深拷贝父代课表
            string json = JsonConvert.SerializeObject(parent1, jsonSettings);
            var child = JsonConvert.DeserializeObject<ClassSchedule>(json, jsonSettings);

            if(child == null)
                return parent1;

            var random = new Random();

            // 随机选择交叉点
            int crossoverPoint = random.Next(child.Sections.Count);

            // 从第二个父代交叉部分的节次替换子代的节次
            for(int i = crossoverPoint; i < child.Sections.Count; i++) {
                // 如果目标节次没有内容，则直接复制
                if(!child.Sections[i].Contents.Any()) {
                    var parentSection = parent2.Sections[i];
                    child.Sections[i].Contents = new List<SectionContent>(parentSection.Contents);
                }
            }

            return child;
        }

        /// <summary>
        /// 变异课表
        /// </summary>
        /// <param name="schedule">原始课表</param>
        /// <param name="baseSchedule">基础课表模板</param>
        /// <param name="courses">待分配课程</param>
        /// <param name="constraint">约束条件</param>
        /// <returns>变异后的课表</returns>
        private ClassSchedule MutateSchedule(
            ClassSchedule schedule,
            ClassSchedule baseSchedule,
            List<IRule> courses,
            List<IRule>? constraint) {
            string json = JsonConvert.SerializeObject(schedule, jsonSettings);
            var mutatedSchedule = JsonConvert.DeserializeObject<ClassSchedule>(json, jsonSettings);
            if(mutatedSchedule == null)
                return schedule;

            var random = new Random();

            // 随机选择一个需要变异的节次
            var mutableSections = mutatedSchedule.Sections
                .Where(s => s.Contents.Any())
                .ToList();

            if(!mutableSections.Any())
                return mutatedSchedule;

            var sectionToMutate = mutableSections[random.Next(mutableSections.Count)];
            var courseToRelocate = sectionToMutate.Contents.First().Content;
            if(courseToRelocate == null)
                return mutatedSchedule;

            // 从节次中移除课程
            sectionToMutate.Contents.Clear();

            // 尝试在新的可用节次中重新放置课程
            var availableSections = mutatedSchedule.Sections
                .Where(s => !s.Contents.Any() &&
                            s.Status == SectionStatus.Normal &&
                            s.LinkTo == null &&
                            !s.IsMergeCell)
                .ToList();

            if(availableSections.Any()) {
                var newSection = availableSections[random.Next(availableSections.Count)];

                // 检查是否可以分配
                Tuple<bool, string> tupleAssign = schedule.CanAssign(newSection, courseToRelocate, constraint);
                if(!tupleAssign.Item1) {
                    newSection.AddSectionContent(new SectionContent(0, courseToRelocate));
                }
                else {
                    // 如果不能分配，则将课程放回原节次
                    sectionToMutate.AddSectionContent(new SectionContent(0, courseToRelocate));
                }
            }
            else {
                // 如果没有可用节次，则将课程放回原节次
                sectionToMutate.AddSectionContent(new SectionContent(0, courseToRelocate));
            }

            return mutatedSchedule;
        }
    }

    public class EvaluatedSchedule {
        public ClassSchedule Schedule { get; set; }
        public double Fitness { get; set; }
    }
}