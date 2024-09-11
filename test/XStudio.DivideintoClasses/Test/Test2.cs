using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.DivideintoClasses.Test
{

    public class Test2 {
        public static void Run()
        {
            List<Course> courses = new List<Course>
        {
            new Course { Name = "Math", Duration = 2, Teachers = new List<string> { "Alice", "Bob" } },
            new Course { Name = "Science", Duration = 2, Teachers = new List<string> { "Alice", "Charlie" } },
            new Course { Name = "History", Duration = 1, Teachers = new List<string> { "Bob", "Charlie" } }
        };

            List<Teacher> teachers = new List<Teacher>
        {
            new Teacher { Name = "Alice", AvailableSlots = Enumerable.Range(0, 10).ToList() },
            new Teacher { Name = "Bob", AvailableSlots = Enumerable.Range(0, 10).ToList() },
            new Teacher { Name = "Charlie", AvailableSlots = Enumerable.Range(0, 10).ToList() }
        };

            int totalSlots = 10;
            GeneticAlgorithm ga = new GeneticAlgorithm(courses, teachers, totalSlots);
            Schedule bestSchedule = ga.Run(populationSize: 50, generations: 100, mutationRate: 0.01);

            foreach (var entry in bestSchedule.TimeTable)
            {
                Console.WriteLine($"Slot {entry.Key}: {entry.Value.Item1.Name} by {entry.Value.Item2}");
            }
        }
    }

    public class Course
    {
        public string Name { get; set; }
        public int Duration { get; set; } // in hours
        public List<string> Teachers { get; set; }
    }

    public class Teacher
    {
        public string Name { get; set; }
        public List<int> AvailableSlots { get; set; } // available time slots
    }

    public class Schedule
    {
        public Dictionary<int, (Course, string)> TimeTable { get; set; } // time slot -> (course, teacher)
    }

    public class GeneticAlgorithm
    {
        private List<Course> courses;
        private List<Teacher> teachers;
        private int totalSlots;
        private Random random;

        public GeneticAlgorithm(List<Course> courses, List<Teacher> teachers, int totalSlots)
        {
            this.courses = courses;
            this.teachers = teachers;
            this.totalSlots = totalSlots;
            this.random = new Random();
        }

        public Schedule Run(int populationSize, int generations, double mutationRate)
        {
            List<Schedule> population = InitializePopulation(populationSize);
            for (int i = 0; i < generations; i++)
            {
                population = EvolvePopulation(population, mutationRate);
            }
            return population.OrderBy(s => Fitness(s)).First();
        }

        private List<Schedule> InitializePopulation(int populationSize)
        {
            List<Schedule> population = new List<Schedule>();
            for (int i = 0; i < populationSize; i++)
            {
                population.Add(CreateRandomSchedule());
            }
            return population;
        }

        private Schedule CreateRandomSchedule()
        {
            Schedule schedule = new Schedule { TimeTable = new Dictionary<int, (Course, string)>() };
            foreach (var course in courses)
            {
                int slot = random.Next(totalSlots);
                string teacher = course.Teachers[random.Next(course.Teachers.Count)];
                schedule.TimeTable[slot] = (course, teacher);
            }
            return schedule;
        }

        private List<Schedule> EvolvePopulation(List<Schedule> population, double mutationRate)
        {
            List<Schedule> newPopulation = new List<Schedule>();
            for (int i = 0; i < population.Count; i++)
            {
                Schedule parent1 = SelectParent(population);
                Schedule parent2 = SelectParent(population);
                Schedule child = Crossover(parent1, parent2);
                if (random.NextDouble() < mutationRate)
                {
                    Mutate(child);
                }
                newPopulation.Add(child);
            }
            return newPopulation;
        }

        private Schedule SelectParent(List<Schedule> population)
        {
            return population[random.Next(population.Count)];
        }

        private Schedule Crossover(Schedule parent1, Schedule parent2)
        {
            Schedule child = new Schedule { TimeTable = new Dictionary<int, (Course, string)>() };
            foreach (var slot in parent1.TimeTable.Keys)
            {
                if (random.NextDouble() < 0.5)
                {
                    child.TimeTable[slot] = parent1.TimeTable[slot];
                }
                else if (parent2.TimeTable.ContainsKey(slot))
                {
                    child.TimeTable[slot] = parent2.TimeTable[slot];
                }
                else
                {
                    child.TimeTable[slot] = parent1.TimeTable[slot];
                }
            }
            return child;
        }

        private void Mutate(Schedule schedule)
        {
            int slot = random.Next(totalSlots);
            var course = courses[random.Next(courses.Count)];
            string teacher = course.Teachers[random.Next(course.Teachers.Count)];
            schedule.TimeTable[slot] = (course, teacher);
        }

        private int Fitness(Schedule schedule)
        {
            // Implement fitness function based on constraints and optimization goals
            int fitness = 0;

            // 约束条件：教师的可用时间
            foreach (var entry in schedule.TimeTable)
            {
                int slot = entry.Key;
                string teacher = entry.Value.Item2;
                var teacherObj = teachers.FirstOrDefault(t => t.Name == teacher);
                if (teacherObj != null && !teacherObj.AvailableSlots.Contains(slot))
                {
                    fitness += 100; // 违反约束条件，增加惩罚
                }
            }

            // 约束条件：课程冲突
            var groupedBySlot = schedule.TimeTable.GroupBy(e => e.Key);
            foreach (var group in groupedBySlot)
            {
                if (group.Count() > 1)
                {
                    fitness += 100; // 违反约束条件，增加惩罚
                }
            }

            return fitness;
        }
    }
}
