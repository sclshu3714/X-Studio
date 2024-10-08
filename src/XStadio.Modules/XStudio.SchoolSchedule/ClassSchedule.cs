﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;
using XStudio.SchoolSchedule.Rules;
using static System.Net.Mime.MediaTypeNames;

namespace XStudio.SchoolSchedule
{
    /// <summary>
    /// 班级课表
    /// </summary>
    /// <example>
    /// 数据量：一周上课节次数 * 教学周 => 12 * 7 * 15
    /// </example>
    //public class ClassSchedule
    //{
    //    /// <summary>
    //    /// 节次内容
    //    /// </summary>
    //    public Section[,] Sections { get; set; }
    //    public ClassSchedule(int days, int period)
    //    {
    //        Sections = new Section[days, period];
    //    }
    //    /// <summary>
    //    /// 索引器定义(获取第Index节课的所有星期的节次)
    //    /// </summary>
    //    /// <param name="index">一周内的所有这节次</param>
    //    /// <returns>节次信息</returns>
    //    public List<Section> this[int index]
    //    {
    //        get
    //        {
    //            List<Section> result = new List<Section>();
    //            for (int i = 0; i < Sections.Length; i++)
    //            {
    //                Section? section = Sections[i, index];
    //                if (section != null)
    //                {
    //                    result.Add(section);
    //                }
    //            }
    //            return result;
    //        }
    //    }
    //    /// <summary>
    //    /// 索引器定义(获取星期week的所有节次)
    //    /// </summary>
    //    /// <param name="week">星期几的全部节次</param>
    //    /// <returns>节次信息</returns>
    //    public List<Section> this[DayOfWeek day]
    //    {
    //        get
    //        {
    //            List<Section> result = new List<Section>();
    //            for (int i = 0; i < Sections.GetLength((int)day); i++)
    //            {
    //                Section? section = Sections[(int)day, i];
    //                if (section != null)
    //                {
    //                    result.Add(section);
    //                }
    //            }
    //            return result;
    //        }
    //    }
    //    /// <summary>
    //    /// 索引器定义(获取第Index节课星期week的课)
    //    /// </summary>
    //    /// <param name="index">第几节</param>
    //    /// <param name="week">星期几</param>
    //    /// <returns>节次信息</returns>
    //    public Section? this[DayOfWeek day, int index]
    //    {
    //        get
    //        {
    //            return Sections[(int)day, index];
    //        }
    //    }        
    //    public void SetSection(DayOfWeek day, int index, Section section)
    //    {
    //        Sections[(int)day, index] = section;
    //    }
    //}


    /// <summary>
    /// 班级课表
    /// </summary>
    /// <example>
    /// 数据量：一周上课节次数 * 教学周 => 12 * 7 * 15
    /// </example>
    public class ClassSchedule
    {
        public ClassSchedule()
        {

        }


        /// <summary>
        /// 默认周一 - 周日,当然也可以控制周日 - 周六,周六 - 周五等
        /// </summary>
        public List<DayOfWeek> LayoutOfWeek { get; set; } = new List<DayOfWeek>() {
             DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
             DayOfWeek.Friday, DayOfWeek.Saturday,DayOfWeek.Sunday
        };

        /// <summary>
        /// 最大节次
        /// </summary>
        public int MaxPeriod { get; set; } = 1;


        /// <summary>
        /// 节次内容
        /// </summary>
        public List<Section> Sections { get; set; } = new List<Section>();

        /// <summary>
        /// 索引器定义(获取第Index节课的所有星期的节次)
        /// </summary>
        /// <param name="period">一周内的所有这节次</param>
        /// <returns>节次信息</returns>
        public IEnumerable<Section> this[int period]
        {
            get
            {
                return Sections.Where(s => s.Period == period);
            }
        }

        /// <summary>
        /// 索引器定义(获取第Index节课的所有星期的节次)
        /// </summary>
        /// <param name="periods">一周内的所有这节次</param>
        /// <returns>节次信息</returns>
        public IEnumerable<Section> this[IEnumerable<int> periods]
        {
            get
            {
                return Sections.Where(s => periods.Contains(s.Period));
            }
        }

        /// <summary>
        /// 通过节次代码获取节次
        /// </summary>
        /// <param name="code">节次代码</param>
        /// <returns>节次</returns>
        public Section this[string code]
        {
            get
            {
                return Sections.First(s => s.Code == code);
            }
        }


        /// <summary>
        /// 索引器定义(获取星期week的所有节次)
        /// </summary>
        /// <param name="week">星期几的全部节次</param>
        /// <returns>节次信息</returns>
        public IEnumerable<Section> this[DayOfWeek day]
        {
            get
            {
                return Sections.Where(s => s.Day == day);
            }
        }

        /// <summary>
        /// 不包括day的节次
        /// </summary>
        /// <param name="day"></param>
        /// <param name="period"></param>
        /// <param name="columnCount"></param>
        /// <returns></returns>
        public IEnumerable<Section> this[int period, DayOfWeek day,int columnCount] 
        {
            get
            {
                return Sections.Where(s => s.Period == period && s.Day > day && s.Day < day + columnCount);
            }
        }

        /// <summary>
        /// 不包括period的节次
        /// </summary>
        /// <param name="day"></param>
        /// <param name="period"></param>
        /// <param name="columnCount"></param>
        /// <returns></returns>
        public IEnumerable<Section> this[DayOfWeek day, int period, int rowCount]
        {
            get
            {
                return Sections.Where(s => s.Day == day && s.Period > period && s.Period < period + rowCount);
            }
        }


        /// <summary>
        /// 索引器定义(获取第period节课星期week的课)
        /// </summary>
        /// <param name="period">第几节</param>
        /// <param name="week">星期几</param>
        /// <returns>节次信息</returns>
        /// <example>
        /// 星期天第3节课 = this[DayOfWeek.Sunday, 3]
        /// </example>
        public Section this[DayOfWeek day, int period]
        {
            get
            {
                return Sections.First(s => s.Day == day && s.Period == period);
            }
        }

        /// <summary>
        /// 索引器定义(获取第period节课星期week的课)
        /// </summary>
        /// <param name="period">第几节</param>
        /// <param name="week">星期几</param>
        /// <returns>节次信息</returns>
        /// <example>
        /// 星期天第3节课 = this[7, 3]
        /// </example>
        public Section this[int iday, int period]
        {
            get
            {
                return Sections.First(s => (int)s.Day == iday && s.Period == period);
            }
        }

        /// <summary>
        ///  初始化课表
        ///     先设置LayoutOfWeek，不设置将使用默认【周一 - 周日】模式, 不在LayoutOfWeek内的节次将设置为不启用
        ///     该方法初始化后，没有设置时段和节次类型，再次设置时段和节次类型
        /// </summary>
        /// <param name="maxPeriod">最大节次</param>
        public bool InitializeSchedule(int maxPeriod)
        {
            MaxPeriod = maxPeriod;
            for (int d = 1; d <= 7; d++)
            {
                DayOfWeek day = (DayOfWeek)d;
                for (int i = 1; i <= maxPeriod; i++)
                {
                    Section section = new Section();
                    section.Day = day;
                    section.Period = i;
                    section.Status = LayoutOfWeek.Contains(day) ? SectionStatus.Normal : SectionStatus.NotEnabled;
                    section.SetSectionCode(section.Period, day);
                    AddSection(section);
                }
            }
            return true;
        }

        /// <summary>
        /// 设置时段和节次类型
        /// </summary>
        /// <param name="periods">节次数组</param>
        /// <param name="TimePeriod">时段</param>
        /// <param name="sectionType">节次类型</param>
        /// <example>
        /// [1,2] 早晨 早读
        /// [3,4,6,7] 上午 正课
        /// [5] 上午 课件活动
        /// </example>
        public void SetSectionTimePeriod(IEnumerable<int> periods,string TimePeriod, SectionType sectionType)
        {
            foreach (var item in this[periods])
            {
                item.TimePeriod = TimePeriod; 
                item.Type = sectionType;
            }
        }

        /// <summary>
        /// 设置节次的启动和结束时间
        /// </summary>
        /// <param name="periods"></param>
        /// <param name="TimePeriod"></param>
        /// <param name="sectionType"></param>
        public void SetSectionTimeSpan(int period, TimeSpan start, TimeSpan end)
        {
            foreach (var item in this[period])
            {
                item.Start = start;
                item.End = end;
            }
        }

        /// <summary>
        /// 设置节次的名字
        /// </summary>
        /// <param name="period"></param>
        /// <param name="name"></param>
        public void SetSectionName(int period,string name)
        {
            foreach (var item in this[period])
            {
                item.Name = name;
            }
        }

        /// <summary>
        /// 设置横向合并单元格（通栏）
        /// </summary>
        /// <param name="day">星期几开始</param>
        /// <param name="period">节次</param>
        /// <param name="columnSpan">合并节次</param>
        /// <example>
        /// 星期一 第3节 合并7 = 第3节课，周一 - 周日 通栏
        /// </example>
        public void SetColumnSpan(DayOfWeek day, int period, int columnSpan)
        {
            int span = (int)day + columnSpan - 1;
            Section section = this[day, period];
            IEnumerable<Section> Sections = this[period, day, columnSpan];
            if (Sections.Any(it=>(it.ColumnSpan > 1 || it.RowSpan > 1) && it.LinkTo != null))
            {
                return;
            }
            section.ColumnSpan = columnSpan;
            for (int i = (int)day + 1; i <= span; i++)
            {
                Section item = this[i, period];
                item.LinkTo = section;
            }
        }


        /// <summary>
        /// 设置纵向合并单元格（通栏）
        /// </summary>
        /// <param name="day">星期几</param>
        /// <param name="period">起始节次</param>
        /// <param name="rowSpan">合并节次</param>
        public void SetRowSpan(DayOfWeek day, int period, int rowSpan)
        {
            int span = period + rowSpan - 1;
            Section section = this[day, period];
            IEnumerable<Section> Sections = this[day, period, rowSpan];
            if (Sections.Any(it => (it.ColumnSpan > 1 || it.RowSpan > 1) && it.LinkTo != null))
            {
                return;
            }
            section.RowSpan = rowSpan;
            for (int i = period + 1; i <= span; i++)
            {
                Section item = this[i, period];
                item.LinkTo = section;
            }
        }


        /// <summary>
        /// 添加节次到课表
        /// </summary>
        /// <param name="section"></param>
        public void AddSection(Section section)
        {
            Sections.Add(section);
        }

        /// <summary>
        /// 向节次中安排内容
        /// </summary>
        /// <param name="code"></param>
        /// <param name="content"></param>
        public void AddSectionContent(string code, SectionContent content)
        {
            this[code].AddSectionContent(content);
        }

        /// <summary>
        /// 验证整个表格中可以放置的位置(抬起验证)
        /// </summary>
        /// <param name="sourceCode">要移动的节次代码</param>
        /// <returns>返回可以放置的课表，或空值</returns>
        public async Task<ClassSchedule?> VerifyExchangeSessionsAsync(string sourceCode)
        {
            // 备份当前课表
            ClassSchedule? schedule = JsonConvert.DeserializeObject<ClassSchedule>(JsonConvert.SerializeObject(this));
            if(schedule == null || !schedule.Sections.Any())
            {
                return null;
            }
            // 查询所有规则，规则等级第一个最高
            IEnumerable<IRule> rules = new List<IRule>();
            // 当前要移动的节次
            Section? sourceSection = this[sourceCode];
             if (sourceSection == null)
                {
                    return null; // 源节次不存在
                }
            // 判断可以移动到的位置
            // 1. 未启用的全部设置为灰(默认状态，无需验证)，不可以放置
            // 2. 已经禁用或者锁定的的节次(默认状态，无需验证), 不可以放置
            // 3. 空白的相同类型的都可以放置(默认状态，无需验证)
            // 4. 只能节次类型相同的放置，不同类型的设置为禁用状态
            // 5. 验证规则通过的可以放置
            //    规则：
            //    1. 源节次占格子必须和目标节次占格子相同
            //    2. 源节次放置到目标节次位置必须满足规则(只能排、不能排、不相邻等规则)
            foreach (var targetSection in schedule.Sections)
            {
                // 规则1: 未启用的节次直接设置为禁用
                if (targetSection.Status != SectionStatus.Normal ||
                    targetSection.Type != sourceSection.Type)
                {
                    targetSection.Status = SectionStatus.Disable;
                    continue;
                }

                // 规则5: 验证具体规则是否可以放置
                bool canPlace = (
                    sourceSection.ColumnSpan == targetSection.ColumnSpan &&
                    sourceSection.RowSpan == targetSection.RowSpan &&
                    !targetSection.Contents.Any(x => 
                        x.Content?.Type == RuleType.CanOnlyArrange || 
                        x.Content?.Type == RuleType.CannotBeArranged || 
                        x.Content?.Type == RuleType.ContinuousClasses)
                );

                targetSection.Status = canPlace ? SectionStatus.Normal : SectionStatus.Disable;
            }
            await Task.CompletedTask;
            return schedule;
        }

        /// <summary>
        /// 交换节次
        ///     既是交换两个节次的内容
        /// </summary>
        /// <param name="sourceCode">源节次代码</param>
        /// <param name="targetCode">目标节次代码</param>
        public async Task<Tuple<bool, string>> ExchangeSessionsAsync(string sourceCode, string targetCode)
        {
            Section? sourceSection = this[sourceCode];
            Section? targetSection = this[targetCode];
            return await ExchangeSessionsAsync(sourceSection, targetSection);
        }
        /// <summary>
        /// 交换节次
        ///     既是交换两个节次的内容
        /// </summary>
        /// <param name="sourceSection">源节次</param>
        /// <param name="targetSection">目标节次</param>
        public async Task<Tuple<bool, string>> ExchangeSessionsAsync(Section? sourceSection, Section? targetSection)
        {
            if (sourceSection == null)
            {
                return new Tuple<bool, string>(false, "源节次为空不能交换");
            }
            if (targetSection == null)
            {
                return new Tuple<bool, string>(false, "目标节次为空不能交换");
            }
            List<SectionContent> tempSectionContents = new List<SectionContent>(sourceSection.Contents);
            // 更新源节次的内容
            sourceSection.Contents = new List<SectionContent>(targetSection.Contents);
            // 更新目标节次的内容
            targetSection.Contents = new List<SectionContent>(tempSectionContents);
            await Task.CompletedTask;
            return new Tuple<bool, string>(true, "交换完成");
        }

    }
}
