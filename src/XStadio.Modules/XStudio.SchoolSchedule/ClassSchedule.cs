using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<Section> Sections { get; set; } = new List<Section>();


        /// <summary>
        /// 索引器定义(获取第Index节课的所有星期的节次)
        /// </summary>
        /// <param name="week">星期</param>
        /// <returns>节次信息</returns>
        public List<Section> this[int index]
        {
            get
            {
                return Sections.FindAll(s => s.Index == index);
            }
        }

        /// <summary>
        /// 索引器定义(获取星期week的所有节次)
        /// </summary>
        /// <param name="week">星期</param>
        /// <returns>节次信息</returns>
        public List<Section> this[DayOfWeek week]
        {
            get
            {
                return Sections.FindAll(s => s.Week == week);
            }
        }

        /// <summary>
        /// 索引器定义(获取第Index节课星期week的课)
        /// </summary>
        /// <param name="index">第几节课</param>
        /// <param name="week">星期几</param>
        /// <returns>节次信息</returns>
        public Section? this[int index, DayOfWeek week]
        {
            get
            {
                return Sections.FirstOrDefault(s => s.Index == index && s.Week == week);
            }
        }

        /// <summary>
        /// 索引器定义(获取第Index节课星期week的课)
        /// </summary>
        /// <param name="teachingWeek">教学周</param>
        /// <param name="index">第几节课</param>
        /// <param name="week">星期几</param>
        /// <returns>节次信息</returns>
        public Section? this[int teachingWeek, int index, DayOfWeek week]
        {
            get
            {
                return Sections.FirstOrDefault(s => s.TeachingWeek == teachingWeek && s.Index == index && s.Week == week);
            }
        }

        /// <summary>
        /// 添加节次
        /// </summary>
        /// <param name="section"></param>
        public void AddSection(Section section)
        {
            Sections.Add(section);
        }
    }
}
