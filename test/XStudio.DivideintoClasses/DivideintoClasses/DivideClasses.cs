using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using XStudio.DivideintoClasses.DivideintoClasses;

namespace MyConsoleApp.DivideintoClasses
{
    /// <summary>
    /// 分班
    /// </summary>
    public class DivideClasses
    {
        //mobile student          移动上课学生
        // itinerant student      流动学生
        private static Random _random = new Random();

        /// <summary>
        /// 所有学生
        /// </summary>
        public static List<Student> TotalStudents = new List<Student>();
        /// <summary>
        /// 行政班
        /// </summary>
        public static List<AdministrativeClass> AClasses = new List<AdministrativeClass>();

        /// <summary>
        /// 教学班
        /// </summary>
        public static List<InstructionalClass> IClasses = new List<InstructionalClass>();
        /// <summary>
        /// 已完成分配的行政班
        /// </summary>
        public static List<AdministrativeClass> AssignedClasses = new List<AdministrativeClass>();

        /// <summary>
        /// 没有安排满学生的班级，没有达到开课标准的班级
        /// </summary>
        public static List<AdministrativeClass> ShortageStaffClasses = new List<AdministrativeClass>();

        /// <summary>
        /// 还没有安排的学生
        /// </summary>
        public static Dictionary<ExaminationType, List<Student>> UnallocatedStudents = new Dictionary<ExaminationType, List<Student>>();

        /// <summary>
        /// 缓存数据
        /// </summary>
        public static ObjectCache Cache = MemoryCache.Default;
        public static IEnumerable<Student> SimulateStudentGeneration(int studentNum)
        {
            for (int i = 0; i < studentNum; i++)
            {
                yield return new Student()
                {
                    FirstName = "A",
                    LastName = $"{i}",
                    Sex = _random.Next(0, 2),
                    Exams = GetRandomExamType()
                };
            }
        }
        public static ExaminationType GetRandomExamType()
        {
            // 定义权重数组
            ExaminationType[] weightedExams = new ExaminationType[]
            {
                ExaminationType.PCB, ExaminationType.PCB, ExaminationType.PCB, ExaminationType.PCB, ExaminationType.PCB,
                ExaminationType.PCB, ExaminationType.PCB, ExaminationType.PCB, ExaminationType.PCB, ExaminationType.PCB,
                ExaminationType.PCB, ExaminationType.PCB, ExaminationType.PCB, ExaminationType.PCB, ExaminationType.PCB,
                ExaminationType.PCB, ExaminationType.PCB, ExaminationType.PCB, ExaminationType.PCB, ExaminationType.PCB,
                ExaminationType.PCB, ExaminationType.PCB, ExaminationType.PCB, ExaminationType.PCB, ExaminationType.PCB,// 50%
                ExaminationType.HOG, ExaminationType.HOG, ExaminationType.HOG, ExaminationType.HOG, ExaminationType.HOG,
                ExaminationType.HOG, ExaminationType.HOG, ExaminationType.HOG, ExaminationType.HOG, ExaminationType.HOG,
                ExaminationType.HOG, ExaminationType.HOG, ExaminationType.HOG, ExaminationType.HOG, ExaminationType.HOG,// 30%
                ExaminationType.PCO, ExaminationType.PCO, ExaminationType.PCO, ExaminationType.PCO, ExaminationType.PCO,
                ExaminationType.PCG, ExaminationType.PBO, ExaminationType.PBG, ExaminationType.POG,
                ExaminationType.HOC, ExaminationType.HOB, ExaminationType.HGC, ExaminationType.HGB, ExaminationType.HCB // 20%
            };

            // 随机选择一个
            int index = _random.Next(weightedExams.Length);
            return weightedExams[index];
        }

        /// <summary>
        /// 给教室分配人数
        /// </summary>
        /// <param name="count"></param>
        /// <param name="theClassCount"></param>
        /// <param name="iRatedClassSize">参考人数</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static List<Tuple<int, int, int>> OnDistributeStudents(int totalStudentCount, int boyStudents, int grilStudents, int classCount, int minClassSize, int maxClassSize)
        {
            int baseSize = totalStudentCount / classCount;
            int extraSize = totalStudentCount % classCount;
            int baseBoySize = boyStudents / classCount;
            int extraBoySize = boyStudents % classCount;
            int baseGrilSize = grilStudents / classCount;
            int extraGrilSize = grilStudents % classCount;
            List<Tuple<int, int, int>> classSize = new List<Tuple<int, int, int>>();
            for (int i = 0; i < classCount; i++)
            {
                int iClassSize = baseSize + (i < extraSize ? 1 : 0);
                int iClassBoySize = baseBoySize + (i < extraBoySize ? 1 : 0);
                int iClassGrilSize = baseGrilSize + (classCount - i <= extraGrilSize ? 1 : 0);
                classSize.Add(new Tuple<int, int, int>(iClassSize, iClassBoySize, iClassGrilSize));
            }
            return classSize;
        }

        /// <summary>
        /// 比较器
        /// </summary>
        /// <param name="group1"></param>
        /// <param name="group2"></param>
        /// <returns></returns>
        private int CommonLetters(string group1, string group2)
        {
            return group1.Intersect(group2).Count();
        }

        /// <summary>
        /// 示例：
        /// 有950个学生，需要安排到21个班级中，其中有PCB,PCO,PCG,PBO,PBG,POG,HOG,HOC,HOB,HGC,HGB,HCB这12个组，
        /// 所有学生均在这12个组中，各组分别有学生：N1,N2,N3,N4,N5,N6,N7,N8,N9,N10,N11,N12; 
        /// 每个班最少人数为MinN,最多人数MaxN
        /// 要求：
        ///     1、尽可能让每个组的人生分配到同一个班级
        ///     2、如果条件1分配完成后，剩余的则按照组和包含相同2个字母的组合并分配
        ///     3、当条件1和条件2执行完成后，任然有剩余学生和班级，那么剩余的则按照组和包含相同1个字母的组合并分配
        ///     4、最后如果还有剩余，则补充到没有满员的组合班级内
        /// </summary>
        /// <param name="studentNum"></param>
        /// <param name="classNum"></param>
        /// <param name="minClassSize"></param>
        /// <param name="maxClassSize"></param>
        /// <returns></returns>
        public static bool StartDivide(int studentNum, int classNum, int minClassSize, int maxClassSize)
        {
            AClasses.Clear();
            IClasses.Clear();
            AssignedClasses.Clear();
            ShortageStaffClasses.Clear();
            UnallocatedStudents.Clear();
            //if (Cache.Contains("TotalStudents"))
            //{
            //    TotalStudents = (List<Student>)Cache["TotalStudents"];
            //}
            //else
            //{
            //    TotalStudents = SimulateStudentGeneration(studentNum).ToList();
            //    Cache.Add("TotalStudents", TotalStudents, new CacheItemPolicy() { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(10) });
            //}
            TotalStudents = SimulateStudentGeneration(studentNum).ToList();
            if (TotalStudents == null || !TotalStudents.Any())
            {
                return false;
            }
            //分班
            doDistributeStudents(classNum, minClassSize, maxClassSize);

            Console.WriteLine($"========================================= 统 计 ==============================================");
            Console.WriteLine($" ");
            foreach (var item in AClasses)
            {
                Console.WriteLine($"未安排的行政班：班级名称：{item.Name}，班级组合：{string.Join("&", item.ExamsList)}, 班级状态：{item.Status}, 学生人数: {item.CurrentClassSize}, 男生人数：{item.BoyNumber}, 女生人数：{item.GrilNummber}, 班级规定人数：{item.MinClassSize} - {item.MaxClassSize}");
            }
            Console.WriteLine($" ");
            Console.WriteLine($"-----------------------------------------------------------------------------------------------");
            Console.WriteLine($" ");
            foreach (var item in AssignedClasses)
            {
                Console.WriteLine($"安排完成的行政班：班级名称：{item.Name}，班级组合：{string.Join("&", item.ExamsList)}, 班级状态：{item.Status}, 学生人数: {item.CurrentClassSize}, 男生人数：{item.BoyNumber}, 女生人数：{item.GrilNummber}, 班级规定人数：{item.MinClassSize} - {item.MaxClassSize}");
            }
            Console.WriteLine($" ");
            Console.WriteLine($"-----------------------------------------------------------------------------------------------");
            Console.WriteLine($" ");
            foreach (var item in UnallocatedStudents.Keys)
            {
                Console.WriteLine($"未分配学生：选考组合：{item}, 人数：{UnallocatedStudents[item].Count}");
            }
            Console.WriteLine($"学生中人数：{TotalStudents.Count}, 已安排学生：{AClasses.Sum(ac => ac.Students.Count)}，未安排学生：{UnallocatedStudents.Sum(uac => uac.Value.Count)}");
            Console.WriteLine($" ");
            Console.WriteLine($"-----------------------------------------------------------------------------------------------");
            Console.WriteLine($" ");
            Console.WriteLine($"班级数量：{classNum}, 已安排班级：{AssignedClasses.Count}，未安排班级：{AClasses.Count}, 未达到标准的班级：{ShortageStaffClasses.Count}");

            foreach (var item in ShortageStaffClasses)
            {
                Console.WriteLine($"缺员行政班：班级名称：{item.Name}，学生人数: {item.CurrentClassSize}，缺员：{(item.MinClassSize - item.CurrentClassSize)} - {(item.MaxClassSize - item.CurrentClassSize)}");
            }
            Console.WriteLine($"合计缺员：{(ShortageStaffClasses.Sum(s => s.MinClassSize) - ShortageStaffClasses.Sum(s => s.CurrentClassSize))} - {(ShortageStaffClasses.Sum(s => s.MaxClassSize) - ShortageStaffClasses.Sum(s => s.CurrentClassSize))}");
            return true;
        }

        /// <summary>
        /// 分班
        /// </summary>
        /// <param name="classNum"></param>
        /// <param name="minClassSize"></param>
        /// <param name="maxClassSize"></param>
        /// <returns></returns>
        public static void doDistributeStudents(int classNum, int minClassSize, int maxClassSize)
        {
            //创建所有行政班级
            int boyStudents = TotalStudents.Count(stu => stu.Sex == 0);                        // 男学生数量
            int grilStudents = TotalStudents.Count(stu => stu.Sex == 1);                       // 女学生数量
            //List<Tuple<int, int, int>> ClassStudens = OnDistributeStudents(TotalStudents.Count, boyStudents, grilStudents, classNum, minClassSize, maxClassSize);//班级分配人数

            Console.WriteLine($"=======================================================================================");
            Console.WriteLine($"源数据=>班级数量：{classNum}, 学生总数：{TotalStudents.Count}, 男生数量：{boyStudents}, 女生数量：{grilStudents}");
            //Console.WriteLine($"计算后=>班级数量：{ClassStudens.Count}, 学生总数：{ClassStudens.Sum(a => a.Item1)}, 男生数量：{ClassStudens.Sum(a => a.Item2)}, 女生数量：{ClassStudens.Sum(a => a.Item3)}");
            Console.WriteLine($"=======================================================================================");

            for (int i = 1; i <= classNum; i++)
            {
                AdministrativeClass administrativeClass = new AdministrativeClass()
                {
                    Number = i,
                    Name = $"行政{i}班",
                    MinClassSize = minClassSize,
                    MaxClassSize = maxClassSize,
                    RatedClassSize = TotalStudents.Count / classNum
                };
                AClasses.Add(administrativeClass);
            }
            // 分配学生到班级(定3)
            AssignStudentsToClasses1(classNum, minClassSize, maxClassSize);

            // 分配学生到班级(定2)
            while (AssignStudentsToClasses2(classNum, minClassSize, maxClassSize))
            {
                
            }
        }

        private static bool AssignStudentsToClasses2(int classNum, int minClassSize, int maxClassSize)
        {
            //1、先排出定3分配后剩余的学生，如果其它学生能够排满，那么定3剩余的学生补充到定3班级，减少学生走班
            IEnumerable<KeyValuePair<ExaminationType, List<Student>>> NowUnallocatedStudents = UnallocatedStudents.Where(us => !AssignedClasses.Any(ac => ac.ExamsList.Contains(us.Key)));
            //2、
            Dictionary<string, IEnumerable<Student>> results = new Dictionary<string, IEnumerable<Student>>();
            var keys = NowUnallocatedStudents.ToList();
            Dictionary<string, List<ExaminationType>> ExaminationTypes = new Dictionary<string, List<ExaminationType>>();
            for (int i = 0; i < keys.Count; i++)
            {
                string? newKey = null;
                ExaminationType key1 = keys[i].Key;
                List<Student> values1 = keys[i].Value;
                for (int j = i + 1; j < keys.Count; j++)
                {
                    ExaminationType key2 = keys[j].Key;
                    List<Student> values2 = keys[j].Value;
                    IEnumerable<char> Intersects;
                    //if (newKey == null)
                    {
                        Intersects = $"{key1}".Intersect($"{key2}");
                        newKey = string.Join("", Intersects);
                    }
                    //else
                    //{
                    //    Intersects = $"{newKey}".Intersect($"{key2}");
                    //}
                    if (Intersects.Count() == 2 && newKey != null)
                    {
                        if (!results.ContainsKey(newKey))
                        {
                            results.Add(newKey, values1.Union(values2));
                            ExaminationTypes.Add(newKey, new List<ExaminationType>() { key1, key2 });
                        }
                        else
                        {
                            results[newKey] = results[newKey].Union(values1.Union(values2));
                            ExaminationTypes[newKey].Add(key2);
                            ExaminationTypes[newKey] = ExaminationTypes[newKey].Distinct().ToList();
                        }
                    }
                }
            }


            Console.WriteLine($"============================================ 定 2 ===========================================");
            var item = results.OrderByDescending(s => s.Value.Count()).First();
            //foreach (var item in resultDir)
            {
                List<Student> students = item.Value.ToList();
                if (!AClasses.Any() || students.Count < minClassSize)
                {
                    return false;
                }

                // 计算组内需要多少个班级，多少个标准班可以
                int minRatedClassSize = AClasses.Min(a => a.RatedClassSize);
                int maxRatedClassSize = AClasses.Max(a => a.RatedClassSize);
                int theClassCount = students.Count / minRatedClassSize;
                int AStudents = theClassCount * minRatedClassSize;
                if (theClassCount == 0)
                {
                    //当前组的学生不够达到标准班
                    return false;
                }

                // 学生在这个组内的分配情况
                var AStudentList = students.Take(AStudents); // TODO 均布男女后取人数，保证取到的那女生分布均匀
                int boyStudents = AStudentList.Count(stu => stu.Sex == 0);                        // 组内男学生数量
                int grilStudents = AStudentList.Count(stu => stu.Sex == 1);                       // 组内女学生数量
                Console.WriteLine($"需要分配的类型：{item.Key}, 学生总数：{item.Value.Count()}, 男生数量：{boyStudents}, 女生数量：{grilStudents}, 包含组：{string.Join(",", ExaminationTypes[item.Key])}");
                List<Tuple<int, int, int>> ClassStudens = OnDistributeStudents(AStudents, boyStudents, grilStudents, theClassCount, minClassSize, maxClassSize);//班级分配人数
                for (int i = 0; i < theClassCount; i++)
                {
                    AssignStudentsToClasses(ref students, ClassStudens[i].Item1, ClassStudens[i].Item2, ClassStudens[i].Item3, ExaminationTypes[item.Key]);
                }

                if (students.Count >= minClassSize)
                {
                    boyStudents = students.Count(stu => stu.Sex == 0);                        // 组内男学生数量
                    grilStudents = students.Count(stu => stu.Sex == 1);                       // 组内女学生数量
                    AssignStudentsToClasses(ref students, minRatedClassSize, boyStudents, grilStudents, ExaminationTypes[item.Key]);
                }
            }

            Console.WriteLine($"=======================================================================================");
            return true;
        }


        /// <summary>
        /// 分配学生到班级
        /// </summary>
        private static void AssignStudentsToClasses1(int classNum, int minClassSize, int maxClassSize)
        {
            IEnumerable<IGrouping<ExaminationType, Student>> gStudents = TotalStudents.GroupBy(stu => stu.Exams)
                                                                                      .OrderByDescending(g => g.Count());
            foreach (var item in gStudents)
            {
                Console.WriteLine($"选考组合：{item.Key}, 选考人数：{item.Count()}");
            }
            Console.WriteLine($"================================ 开  始 ==================================");

            //定3
            foreach (var item in gStudents)
            {
                ExaminationType exams = item.Key;
                List<Student> students = item.ToList();
                if (!AClasses.Any())
                {
                    UnallocatedStudents[exams] = students;
                    continue;
                }
                // 计算组内需要多少个班级，多少个标准班可以
                int minRatedClassSize = AClasses.Min(a => a.RatedClassSize);
                int maxRatedClassSize = AClasses.Max(a => a.RatedClassSize);
                int theClassCount = students.Count / minRatedClassSize;
                int AStudents = theClassCount * minRatedClassSize;
                if (theClassCount == 0)
                {
                    //当前组的学生不够达到标准班
                    UnallocatedStudents.Add(item.Key, students);
                    continue;
                }
                // 学生在这个组内的分配情况
                var AStudentList = item.Take(AStudents); // TODO 均布男女后取人数，保证取到的那女生分布均匀
                int boyStudents = AStudentList.Count(stu => stu.Sex == 0);                        // 组内男学生数量
                int grilStudents = AStudentList.Count(stu => stu.Sex == 1);                       // 组内女学生数量
                List<Tuple<int, int, int>> ClassStudens = OnDistributeStudents(AStudents, boyStudents, grilStudents, theClassCount, minClassSize, maxClassSize);//班级分配人数
                for (int i = 0; i < theClassCount; i++)
                {
                    AssignStudentsToClasses(ref students, ClassStudens[i].Item1, ClassStudens[i].Item2, ClassStudens[i].Item3, new List<ExaminationType>() { exams });
                }

                if (students.Count >= minClassSize)
                {
                    boyStudents = students.Count(stu => stu.Sex == 0);                        // 组内男学生数量
                    grilStudents = students.Count(stu => stu.Sex == 1);                       // 组内女学生数量
                    AssignStudentsToClasses(ref students, minRatedClassSize, boyStudents, grilStudents, new List<ExaminationType>() { exams });
                }
                else if (students.Count >= minClassSize / 2)
                {//剩余的人生超过半个开班需求
                    boyStudents = students.Count(stu => stu.Sex == 0);                        // 组内男学生数量
                    grilStudents = students.Count(stu => stu.Sex == 1);                       // 组内女学生数量
                    AssignStudentsToClasses(ref students, minRatedClassSize, boyStudents, grilStudents, new List<ExaminationType>() { exams });
                }
                else
                { //剩余的人生小于半个开班需求，考虑将人数分配到已经开好的班级中
                    while (students.Any())
                    {
                        foreach (AdministrativeClass aClass in AssignedClasses)
                        {
                            if (!students.Any())
                                break;
                            Student student = students.First();
                            aClass.Students.Add(student);
                            if (student.Sex == 0)
                                aClass.BoyNumber += 1;
                            else
                                aClass.GrilNummber += 1;
                            students.Remove(student);
                        }
                    }

                }


                if (students.Any())
                {
                    UnallocatedStudents[exams] = students;
                }
            }
        }

        public static void AssignStudentsToClasses(ref List<Student> students, int RatedClassSize, int BoyNumber, int GrilNummber, List<ExaminationType> ExamsList)
        {
            AdministrativeClass administrativeClass = AClasses[0];
            administrativeClass.RatedClassSize = RatedClassSize;
            administrativeClass.BoyNumber = BoyNumber;
            administrativeClass.GrilNummber = GrilNummber;
            administrativeClass.ExamsList = ExamsList;
            doArrangeStudents(ref students, administrativeClass);
            Console.WriteLine($"安排班级：{administrativeClass.Name}，选考组合：{string.Join(",", ExamsList)}, 班级人数:{administrativeClass.RatedClassSize},  男生人数：{BoyNumber}, 女生人数：{GrilNummber}  ---> 剩余学生人数: {students.Count}");
            //检查班级是否安排完成
            if (administrativeClass.Status == ClassStatus.ShortageStaff)
            { //当前行政班没有填充满学生
                ShortageStaffClasses.Add(administrativeClass);
            }
            else
            {
                AssignedClasses.Add(administrativeClass);
            }
            AClasses.Remove(administrativeClass);
        }


        private static int CalculateClassCount(int studentCount, int minClassSize)
        {
            int theClassCount = 1;
            while (theClassCount * minClassSize <= studentCount)
            {
                theClassCount++;
            }
            return theClassCount - 1;
        }


        /// <summary>
        /// 定3
        /// </summary>
        /// <param name="item"></param>
        /// <param name="classNum"></param>
        /// <param name="iRatedClassSize">参考人数</param>
        /// <param name="minClassSize"></param>
        /// <param name="maxClassSize"></param>
        private static void BuildDistributeStudents(IGrouping<ExaminationType, Student> item, int classNum, int minClassSize, int maxClassSize)
        {
            ExaminationType exams = item.Key;
            List<Student> students = item.ToList();                                     // 该组合所有学生
            if (students.Count > minClassSize)
            {
                int boyStudents = students.Count(stu => stu.Sex == 0);                        // 该组合男学生数量
                int grilStudents = students.Count(stu => stu.Sex == 1);                     // 该组合女学生数量
                                                                                            //int theClassCount = Math.Max(1, (int)Math.Round(students.Count * classNum * 1.0f / TotalStudents.Count));
                int theClassCount = Math.Max(1, (int)Math.Round(students.Count * classNum * 1.0f / TotalStudents.Count));
                //验证班级是否合法
                while (theClassCount * minClassSize > students.Count)
                {//验证  班级 * 最小开班人数 > 需要安排的学生数量 
                    theClassCount -= 1;
                }
                //班级人数  男生人数  女生人数
                List<Tuple<int, int, int>> ClassStudens = OnDistributeStudents(students.Count, boyStudents, grilStudents, theClassCount, minClassSize, maxClassSize);//班级分配人数
                Console.WriteLine($"选考组合：{item.Key}, 选考人数：{item.Count()}, 需要班级：{theClassCount}");
                for (int i = 1; i <= theClassCount; i++)
                {
                    if (!AClasses.Any())
                    {
                        //已经安排完了
                        break;
                    }
                    int theClassSize = ClassStudens[i - 1].Item1;
                    //计算分配男生数
                    int ClassBoyStudens = ClassStudens[i - 1].Item2;
                    //计算分片女生数
                    int ClassGrilStudens = ClassStudens[i - 1].Item3;
                    AdministrativeClass administrativeClass = AClasses[0];
                    administrativeClass.MinClassSize = minClassSize;
                    administrativeClass.MaxClassSize = maxClassSize;
                    administrativeClass.BoyNumber = ClassBoyStudens;
                    administrativeClass.GrilNummber = ClassGrilStudens;
                    administrativeClass.ExamsList = new List<ExaminationType>() { exams };
                    //给行政班安排学生 
                    doArrangeStudents(ref students, administrativeClass);

                    //检查班级是否安排完成
                    if (administrativeClass.Status == ClassStatus.ShortageStaff)
                    { //当前行政班没有填充满学生
                        ShortageStaffClasses.Add(administrativeClass);
                    }
                    else
                    {
                        AssignedClasses.Add(administrativeClass);
                    }
                    AClasses.Remove(administrativeClass);
                }
            }
            // 将没有安排完成的学生记录下来
            if (students.Any())
            {
                if (UnallocatedStudents.ContainsKey(item.Key))
                {
                    UnallocatedStudents[item.Key] = students;
                }
                else
                {
                    UnallocatedStudents.Add(item.Key, students);
                }
            }
        }

        /// <summary>
        /// 给行政班安排学生
        /// </summary>
        /// <param name="students">所有可以安排的学生</param>
        /// <param name="administrativeClass">教学班级</param>
        /// <exception cref="NotImplementedException"></exception>
        private static void doArrangeStudents(ref List<Student> students, AdministrativeClass administrativeClass)
        {
            IEnumerable<Student> boyStudents = students.Where(stu => stu.Sex == 0).Take(administrativeClass.BoyNumber);       // 男学生
            IEnumerable<Student> girlStudents = students.Where(stu => stu.Sex == 1).Take(administrativeClass.GrilNummber);      // 女学生
            administrativeClass.Students.AddRange(boyStudents);
            administrativeClass.Students.AddRange(girlStudents);
            foreach (Student student in administrativeClass.Students)
            {
                students.Remove(student);
                if (UnallocatedStudents.ContainsKey(student.Exams))
                {
                    UnallocatedStudents[student.Exams].Remove(student);
                }
            };
        }
    }
}
