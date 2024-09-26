using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.SchoolSchedule
{
    /// <summary>
    /// 班级课表
    /// </summary>
    /// <example>
    /// 数据量：一周上课节次数 * 教学周 => 12 * 7 * 15
    /// </example>
    public class ClassSchedule
    {
        /// <summary>
        /// 节次内容
        /// </summary>
        public Section[,] Sections { get; set; }
        public ClassSchedule(int days, int period)
        {
            Sections = new Section[days, period];
        }

        /// <summary>
        /// 索引器定义(获取第Index节课的所有星期的节次)
        /// </summary>
        /// <param name="index">一周内的所有这节次</param>
        /// <returns>节次信息</returns>
        public List<Section> this[int index]
        {
            get
            {
                List<Section> result = new List<Section>();
                for (int i = 0; i < Sections.Length; i++)
                {
                    Section? section = Sections[i, index];
                    if (section != null)
                    {
                        result.Add(section);
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 索引器定义(获取星期week的所有节次)
        /// </summary>
        /// <param name="week">星期几的全部节次</param>
        /// <returns>节次信息</returns>
        public List<Section> this[DayOfWeek day]
        {
            get
            {
                List<Section> result = new List<Section>();
                for (int i = 0; i < Sections.GetLength((int)day); i++)
                {
                    Section? section = Sections[(int)day, i];
                    if (section != null)
                    {
                        result.Add(section);
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 索引器定义(获取第Index节课星期week的课)
        /// </summary>
        /// <param name="index">第几节</param>
        /// <param name="week">星期几</param>
        /// <returns>节次信息</returns>
        public Section? this[DayOfWeek day, int index]
        {
            get
            {
                return Sections[(int)day, index];
            }
        }
        
        public void SetSection(DayOfWeek day, int index, Section section)
        {
            Sections[(int)day, index] = section;
        }
    }


    /// <summary>
    /// 班级课表
    /// </summary>
    /// <example>
    /// 数据量：一周上课节次数 * 教学周 => 12 * 7 * 15
    /// </example>
    public class ClassScheduleB
    {
        /// <summary>
        /// 节次内容
        /// </summary>
        public List<Section> Sections { get; set; } = new List<Section>();

        /// <summary>
        /// 索引器定义(获取第Index节课的所有星期的节次)
        /// </summary>
        /// <param name="index">一周内的所有这节次</param>
        /// <returns>节次信息</returns>
        public List<Section> this[int index]
        {
            get
            {
                return Sections.Where(s=>s.Index == index).ToList();
            }
        }

        /// <summary>
        /// 索引器定义(获取星期week的所有节次)
        /// </summary>
        /// <param name="week">星期几的全部节次</param>
        /// <returns>节次信息</returns>
        public List<Section> this[DayOfWeek day]
        {
            get
            {
                return Sections.Where(s => s.Day == day).ToList();
            }
        }

        /// <summary>
        /// 索引器定义(获取第Index节课星期week的课)
        /// </summary>
        /// <param name="index">第几节</param>
        /// <param name="week">星期几</param>
        /// <returns>节次信息</returns>
        public Section? this[DayOfWeek day, int index]
        {
            get
            {
                return Sections.FirstOrDefault(s => s.Day == day && s.Index == index);
            }
        }

        public void SetSection(Section section)
        {
            Sections.Add(section);
        }
    }
}
