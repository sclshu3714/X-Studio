using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.DivideintoClasses.Test
{
    public static class GeneticAlgorithmHelper
    {
        public static Random random = new Random();
        public const int PopulationSize = 100;
        public const int Generations = 1000;
        public const double CrossoverRate = 0.7;
        public const double MutationRate = 0.01;
        public static void OnGeneticAlgorithm()
        {
            // 物品的重量和价值
            int[] weights = { 2, 3, 4, 5 };
            int[] values = { 3, 4, 5, 6 };
            int maxWeight = 5;

            // 初始化种群
            List<Individual> population = InitializePopulation(weights.Length);

            for (int generation = 0; generation < Generations; generation++)
            {
                // 评估适应度
                foreach (var individual in population)
                {
                    individual.Fitness = CalculateFitness(individual, weights, values, maxWeight);
                }

                // 选择
                population = Select(population);

                // 交叉和变异
                List<Individual> newPopulation = new List<Individual>();
                while (newPopulation.Count < population.Count)
                {
                    Individual parent1 = population[random.Next(population.Count)];
                    Individual parent2 = population[random.Next(population.Count)];

                    Individual offspring = Crossover(parent1, parent2);
                    Mutate(offspring);
                    newPopulation.Add(offspring);
                }

                population = newPopulation;
            }

            // 找到最佳个体
            var bestIndividual = population.OrderByDescending(i => i.Fitness).First();
            Console.WriteLine($"最佳适应度: {bestIndividual.Fitness}");
        }
        static List<Individual> InitializePopulation(int length)
        {
            List<Individual> population = new List<Individual>();
            for (int i = 0; i < PopulationSize; i++)
            {
                population.Add(new Individual(length));
            }
            return population;
        }

        static double CalculateFitness(Individual individual, int[] weights, int[] values, int maxWeight)
        {
            int totalWeight = 0;
            int totalValue = 0;

            for (int i = 0; i < individual.Genes.Length; i++)
            {
                if (individual.Genes[i] == 1)
                {
                    totalWeight += weights[i];
                    totalValue += values[i];
                }
            }

            return totalWeight <= maxWeight ? totalValue : 0; // 超过最大重量则适应度为0
        }

        static List<Individual> Select(List<Individual> population)
        {
            return population.OrderByDescending(i => i.Fitness).Take(PopulationSize / 2).ToList();
        }

        static Individual Crossover(Individual parent1, Individual parent2)
        {
            Individual offspring = new Individual(parent1.Genes.Length);
            for (int i = 0; i < parent1.Genes.Length; i++)
            {
                offspring.Genes[i] = random.NextDouble() < CrossoverRate ? parent1.Genes[i] : parent2.Genes[i];
            }
            return offspring;
        }

        static void Mutate(Individual individual)
        {
            for (int i = 0; i < individual.Genes.Length; i++)
            {
                if (random.NextDouble() < MutationRate)
                {
                    individual.Genes[i] = individual.Genes[i] == 1 ? 0 : 1; // 反转基因
                }
            }
        }
    }

    class Individual
    {
        public int[] Genes { get; }
        public double Fitness { get; set; }

        public static Random random = new Random();
        public Individual(int length)
        {
            Genes = new int[length];
            for (int i = 0; i < length; i++)
            {
                Genes[i] = random.Next(2); // 随机生成基因（0或1）
            }
        }
    }
}
