﻿// See https://aka.ms/new-console-template for more information
using MyConsoleApp.DivideintoClasses;
using System.Security.Authentication;
using XStudio.DivideintoClasses.Excels;
using XStudio.DivideintoClasses.Test;



Console.WriteLine("Hello, World!");
reDivide:
//Test1.Test();
//GeneticAlgorithmHelper.OnGeneticAlgorithm();
//await ExportToExcelHelper.ExportToExcelWithHeaderStyle();
//await ExportToExcelHelper.ImportExcelToModel("");
DivideClasses.StartDivide(950, 21, 40, 50);
Console.WriteLine("输入ESC退出、其他案件重新计算");
ConsoleKeyInfo keyInfo = Console.ReadKey();
if (keyInfo.Key != ConsoleKey.Escape)
{
    Console.Clear();
    goto reDivide;
}


//using System;
//using System.Collections.Generic;
//using System.Linq;

//class AStudent
//{
//    public int Id { get; set; }
//    public string Gender { get; set; }
//    public string Group { get; set; }
//}

//class Program
//{
//    static Random random = new Random();
//    const int populationSize = 100;
//    const int generations = 100;
//    const int classCount = 21;
//    const int studentsPerClass = 1000 / classCount; // 每个班级的学生数

//    static void Main(string[] args)
//    {
//        var students = InitializeStudents();
//        var population = InitializePopulation(students);

//        for (int i = 0; i < generations; i++)
//        {
//            population = Evolve(population, students);
//        }

//        var bestAllocation = population.OrderByDescending(CalculateFitness).First();
//        PrintAllocation(bestAllocation);
//    }

//    static List<AStudent> InitializeStudents()
//    {
//        // 初始化学生数据（示例数据）
//        var students = new List<AStudent>();
//        // 添加学生数据...
//        return students;
//    }

//    static List<int[]> InitializePopulation(List<AStudent> students)
//    {
//        var population = new List<int[]>();
//        for (int i = 0; i < populationSize; i++)
//        {
//            var allocation = new int[students.Count];
//            for (int j = 0; j < students.Count; j++)
//            {
//                allocation[j] = random.Next(classCount); // 随机分配班级
//            }
//            population.Add(allocation);
//        }
//        return population;
//    }

//    static List<int[]> Evolve(List<int[]> population, List<AStudent> students)
//    {
//        var newPopulation = new List<int[]>();
//        for (int i = 0; i < populationSize; i++)
//        {
//            var parent1 = SelectParent(population, students);
//            var parent2 = SelectParent(population, students);
//            var child = Crossover(parent1, parent2);
//            Mutate(child);
//            newPopulation.Add(child);
//        }
//        return newPopulation;
//    }

//    static int[] SelectParent(List<int[]> population, List<AStudent> students)
//    {
//        return population.OrderByDescending(allocation => CalculateFitness(allocation, students)).Take(2).Last();
//    }

//    static int[] Crossover(int[] parent1, int[] parent2)
//    {
//        int crossoverPoint = random.Next(parent1.Length);
//        var child = new int[parent1.Length];
//        for (int i = 0; i < parent1.Length; i++)
//        {
//            child[i] = i < crossoverPoint ? parent1[i] : parent2[i];
//        }
//        return child;
//    }

//    static void Mutate(int[] allocation)
//    {
//        for (int i = 0; i < allocation.Length; i++)
//        {
//            if (random.NextDouble() < 0.01) // 1% 的突变概率
//            {
//                allocation[i] = random.Next(classCount); // 随机改变班级
//            }
//        }
//    }

//    static int CalculateFitness(int[] allocation, List<AStudent> students)
//    {
//        // 计算适应度：优先考虑相同组的学生分配到同一个班，性别均衡和班级人数相同
//        int fitness = 0;

//        // 计算相同组的学生分配到同一个班的适应度
//        var groupCounts = new Dictionary<string, int[]>();
//        foreach (var AStudent in students)
//        {
//            int classIndex = allocation[AStudent.Id];
//            if (!groupCounts.ContainsKey(AStudent.Group))
//            {
//                groupCounts[AStudent.Group] = new int[classCount];
//            }
//            groupCounts[AStudent.Group][classIndex]++;
//        }

//        foreach (var counts in groupCounts.Values)
//        {
//            fitness += counts.Sum(c => c > 1 ? c : 0); // 计算相同组的适应度
//        }

//        // 计算性别均衡的适应度
//        var genderCounts = new Dictionary<string, int[]>();
//        foreach (var AStudent in students)
//        {
//            int classIndex = allocation[AStudent.Id];
//            if (!genderCounts.ContainsKey(AStudent.Gender))
//            {
//                genderCounts[AStudent.Gender] = new int[classCount];
//            }
//            genderCounts[AStudent.Gender][classIndex]++;
//        }

//        foreach (var counts in genderCounts.Values)
//        {
//            fitness -= counts.Max(); // 减去人数最多的班级
//        }

//        return fitness;
//    }

//    static void PrintAllocation(int[] allocation)
//    {
//        for (int i = 0; i < allocation.Length; i++)
//        {
//            Console.WriteLine($"学生 {i} 分配到班级 {allocation[i]}");
//        }
//    }
//}


//using MyConsoleApp.DivideintoClasses;
//using Org.BouncyCastle.Utilities.Encoders;
//using System;
//using System.Collections.Generic;
//using System.Linq;


//class Program
//{
//    static Random random = new Random();
//    const int populationSize = 100;
//    const int generations = 100;
//    const int classCount = 21;
//    const int studentsPerClass = 1000 / classCount; // 每个班级的学生数

//    static void Main(string[] args)
//    {
//        Console.WriteLine("Hello, World!");
//        var students = InitializeStudents;
//        Console.WriteLine($"学生数量：{students.Count}");

//        var population = InitializePopulation(students);

//        for (int i = 0; i < generations; i++)
//        {
//            population = Evolve(population, students);
//        }

//        var bestAllocation = population.OrderByDescending(allocation=>CalculateFitness(allocation, students)).First();
//        PrintAllocation(bestAllocation);
//        Console.ReadKey();
//    }

//    static List<AStudent> InitializeStudents
//    {
//        get
//        {
//            // 初始化学生数据（示例数据）
//            var students = DivideClasses.SimulateStudentGeneration(1000).ToList();
//            // 添加学生数据...
//            return students;
//        }
//    }

//    static List<int[]> InitializePopulation(List<AStudent> students)
//    {
//        var population = new List<int[]>();
//        for (int i = 0; i < populationSize; i++)
//        {
//            var allocation = new int[students.Count];
//            for (int j = 0; j < students.Count; j++)
//            {
//                allocation[j] = random.Next(classCount); // 随机分配班级
//            }
//            population.Add(allocation);
//        }
//        return population;
//    }

//    static List<int[]> Evolve(List<int[]> population, List<AStudent> students)
//    {
//        var newPopulation = new List<int[]>();
//        for (int i = 0; i < populationSize; i++)
//        {
//            var parent1 = SelectParent(population, students);
//            var parent2 = SelectParent(population, students);
//            var child = Crossover(parent1, parent2);
//            Mutate(child);
//            newPopulation.Add(child);
//        }
//        return newPopulation;
//    }

//    static int[] SelectParent(List<int[]> population, List<AStudent> students)
//    {
//        return population.OrderByDescending(allocation => CalculateFitness(allocation, students)).Take(2).Last();
//    }

//    static int[] Crossover(int[] parent1, int[] parent2)
//    {
//        int crossoverPoint = random.Next(parent1.Length);
//        var child = new int[parent1.Length];
//        for (int i = 0; i < parent1.Length; i++)
//        {
//            child[i] = i < crossoverPoint ? parent1[i] : parent2[i];
//        }
//        return child;
//    }

//    static void Mutate(int[] allocation)
//    {
//        for (int i = 0; i < allocation.Length; i++)
//        {
//            if (random.NextDouble() < 0.01) // 1% 的突变概率
//            {
//                allocation[i] = random.Next(classCount); // 随机改变班级
//            }
//        }
//    }

//    static int CalculateFitness(int[] allocation, List<AStudent> students)
//    {
//        // 计算适应度：优先考虑相同组的学生分配到同一个班，性别均衡和班级人数相同
//        int fitness = 0;

//        // 计算相同组的学生分配到同一个班的适应度
//        var groupCounts = new Dictionary<ExaminationType, int[]>();
//        foreach (var AStudent in students)
//        {
//            int classIndex = allocation[AStudent.Id];
//            if (!groupCounts.ContainsKey(AStudent.Exams))
//            {
//                groupCounts[AStudent.Exams] = new int[classCount];
//            }
//            groupCounts[AStudent.Exams][classIndex]++;
//        }

//        foreach (var counts in groupCounts.Values)
//        {
//            fitness += counts.Sum(c => c > 1 ? c : 0); // 计算相同组的适应度
//        }

//        // 计算性别均衡的适应度
//        var genderCounts = new Dictionary<int, int[]>();
//        foreach (var AStudent in students)
//        {
//            int classIndex = allocation[AStudent.Id];
//            if (!genderCounts.ContainsKey(AStudent.Sex))
//            {
//                genderCounts[AStudent.Sex] = new int[classCount];
//            }
//            genderCounts[AStudent.Sex][classIndex]++;
//        }

//        foreach (var counts in genderCounts.Values)
//        {
//            fitness -= counts.Max(); // 减去人数最多的班级
//        }

//        return fitness;
//    }

//    static void PrintAllocation(int[] allocation)
//    {
//        for (int i = 0; i < allocation.Length; i++)
//        {
//            Console.WriteLine($"学生 {i} 分配到班级 {allocation[i]}");
//        }
//    }
//}




//using System;
//using System.Collections.Generic;
//using System.Linq;

//class AStudent
//{
//    public int Id { get; set; }
//    public string Gender { get; set; }
//    public string Group { get; set; }
//}

//class Program
//{
//    static Random random = new Random();
//    const int populationSize = 100;
//    const int generations = 100;
//    const int classCount = 21;
//    const int minClassSize = 40;
//    const int maxClassSize = 50;

//    static void Main(string[] args)
//    {
//        var students = InitializeStudents();
//        var population = InitializePopulation(students);

//        for (int i = 0; i < generations; i++)
//        {
//            population = Evolve(population, students);
//        }

//        var bestAllocation = population.OrderByDescending(CalculateFitness).First();
//        PrintAllocation(bestAllocation);
//        Console.ReadKey();
//    }

//    static List<AStudent> InitializeStudents()
//    {
//        // 初始化学生数据
//        var students = new List<AStudent>();
//        // 添加物化生组
//        for (int i = 0; i < 423; i++) students.Add(new AStudent { Id = i, Gender = i < 191 ? "男" : "女", Group = "物化生" });
//        // 添加政史地组
//        for (int i = 0; i < 265; i++) students.Add(new AStudent { Id = i + 423, Gender = i < 80 ? "男" : "女", Group = "政史地" });
//        // 添加物化地组
//        for (int i = 0; i < 84; i++) students.Add(new AStudent { Id = i + 688, Gender = i < 49 ? "男" : "女", Group = "物化地" });
//        // 添加政史生组
//        for (int i = 0; i < 64; i++) students.Add(new AStudent { Id = i + 772, Gender = i < 34 ? "男" : "女", Group = "政史生" });
//        // 添加政史化组
//        for (int i = 0; i < 30; i++) students.Add(new AStudent { Id = i + 836, Gender = "女", Group = "政史化" });
//        // 添加物化政组
//        for (int i = 0; i < 134; i++) students.Add(new AStudent { Id = i + 866, Gender = i < 70 ? "男" : "女", Group = "物化政" });

//        return students;
//    }

//    static List<List<List<AStudent>>> InitializePopulation(List<AStudent> students)
//    {
//        var population = new List<List<List<AStudent>>>();
//        for (int i = 0; i < populationSize; i++)
//        {
//            var allocation = new List<List<AStudent>>(new List<AStudent>[classCount]);
//            for (int j = 0; j < classCount; j++)
//            {
//                allocation[j] = new List<AStudent>();
//            }

//            // 随机分配学生
//            foreach (var AStudent in students)
//            {
//                int classIndex = random.Next(classCount);
//                allocation[classIndex].Add(AStudent);
//            }

//            population.Add(allocation);
//        }
//        return population;
//    }

//    static List<List<List<AStudent>>> Evolve(List<List<List<AStudent>>> population, List<AStudent> students)
//    {
//        var newPopulation = new List<List<List<AStudent>>>();
//        for (int i = 0; i < populationSize; i++)
//        {
//            var parent1 = SelectParent(population);
//            var parent2 = SelectParent(population);
//            var child = Crossover(parent1, parent2);
//            Mutate(child);
//            newPopulation.Add(child);
//        }
//        return newPopulation;
//    }

//    static List<List<AStudent>> SelectParent(List<List<List<AStudent>>> population)
//    {
//        return population.OrderByDescending(CalculateFitness).Take(2).Last();
//    }

//    static List<List<AStudent>> Crossover(List<List<AStudent>> parent1, List<List<AStudent>> parent2)
//    {
//        var child = new List<List<AStudent>>(new List<AStudent>[classCount]);
//        for (int i = 0; i < classCount; i++)
//        {
//            child[i] = random.NextDouble() < 0.5 ? parent1[i] : parent2[i];
//        }
//        return child;
//    }

//    static void Mutate(List<List<AStudent>> allocation)
//    {
//        // 随机调整分配
//        foreach (var classList in allocation)
//        {
//            if (random.NextDouble() < 0.1) // 10% 的突变概率
//            {
//                // 随机选择一个学生并重新分配
//                if (classList.Count > 0)
//                {
//                    var AStudent = classList[random.Next(classList.Count)];
//                    classList.Remove(AStudent);
//                    int newClassIndex = random.Next(classCount);
//                    allocation[newClassIndex].Add(AStudent);
//                }
//            }
//        }
//    }

//    static int CalculateFitness(List<List<AStudent>> allocation)
//    {
//        int fitness = 0;

//        // 计算适应度
//        foreach (var classList in allocation)
//        {
//            if (classList.Count >= minClassSize && classList.Count <= maxClassSize)
//            {
//                fitness += 10; // 合法班级加分
//            }

//            var genderCount = classList.GroupBy(s => s.Gender).ToDictionary(g => g.Key, g => g.Count());
//            if (genderCount.TryGetValue("男", out int maleCount) && genderCount.TryGetValue("女", out int femaleCount))
//            {
//                fitness += Math.Min(maleCount, femaleCount); // 男女均分加分
//            }
//        }

//        return fitness;
//    }

//    static void PrintAllocation(List<List<AStudent>> allocation)
//    {
//        for (int i = 0; i < allocation.Count; i++)
//        {
//            Console.WriteLine($"班级 {i + 1}: {allocation[i].Count} 人");
//            foreach (var AStudent in allocation[i])
//            {
//                Console.WriteLine($"  学生 ID: {AStudent.Id}, 性别: {AStudent.Gender}, 组: {AStudent.Group}");
//            }
//        }
//    }
//}