using Newtonsoft.Json;
using XStudio.SchoolSchedule.Constraints;
using XStudio.SchoolSchedule.Enums;
using XStudio.SchoolSchedule.Rules;

namespace XStudio.SchoolSchedule {
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
    public class ClassSchedule {

        public ClassSchedule() {
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
        public IEnumerable<Section> this[int period] {
            get {
                return Sections.Where(s => s.Period == period);
            }
        }

        /// <summary>
        /// 索引器定义(获取第Index节课的所有星期的节次)
        /// </summary>
        /// <param name="periods">一周内的所有这节次</param>
        /// <returns>节次信息</returns>
        public IEnumerable<Section> this[IEnumerable<int> periods] {
            get {
                return Sections.Where(s => periods.Contains(s.Period));
            }
        }

        /// <summary>
        /// 通过节次代码获取节次
        /// </summary>
        /// <param name="code">节次代码</param>
        /// <returns>节次</returns>
        public Section? this[string code] {
            get {
                return Sections.FirstOrDefault(s => s.Code == code);
            }
        }

        /// <summary>
        /// 索引器定义(获取星期week的所有节次)
        /// </summary>
        /// <param name="week">星期几的全部节次</param>
        /// <returns>节次信息</returns>
        public IEnumerable<Section> this[DayOfWeek day] {
            get {
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
        public IEnumerable<Section> this[int period, DayOfWeek day, int columnCount] {
            get {
                int startDay = LayoutOfWeek.IndexOf(day);
                if(startDay + columnCount >= LayoutOfWeek.Count) {
                    columnCount = LayoutOfWeek.Count - startDay;
                }
                List<DayOfWeek> days = LayoutOfWeek.GetRange(startDay, columnCount);
                return Sections.Where(s => s.Period == period && days.Contains(s.Day));
            }
        }

        /// <summary>
        /// 不包括period的节次
        /// </summary>
        /// <param name="day"></param>
        /// <param name="period"></param>
        /// <param name="columnCount"></param>
        /// <returns></returns>
        public IEnumerable<Section> this[DayOfWeek day, int period, int rowCount] {
            get {
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
        public Section? this[DayOfWeek day, int period] {
            get {
                return Sections.FirstOrDefault(s => s.Day == day && s.Period == period);
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
        public Section? this[int iday, int period] {
            get {
                return Sections.FirstOrDefault(s => (int)s.Day == iday && s.Period == period);
            }
        }

        /// <summary>
        ///  初始化课表
        ///     先设置LayoutOfWeek，不设置将使用默认【周一 - 周日】模式, 不在LayoutOfWeek内的节次将设置为不启用
        ///     该方法初始化后，没有设置时段和节次类型，再次设置时段和节次类型
        /// </summary>
        /// <param name="maxPeriod">最大节次</param>
        public bool InitializeSchedule(int maxPeriod) {
            MaxPeriod = maxPeriod;
            foreach(var day in LayoutOfWeek) {
                for(int i = 1; i <= maxPeriod; i++) {
                    Section section = new Section();
                    section.Day = day;
                    section.Period = i;
                    section.Status = SectionStatus.Normal;
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
        /// <param name="isTeaching">是：授课，否：非授课</param>
        /// <example>
        /// [1,2] 早晨 早读
        /// [3,4,6,7] 上午 正课
        /// [5] 上午 课件活动
        /// </example>
        public void SetSectionTimePeriod(IEnumerable<int> periods, string TimePeriod, SectionType sectionType, bool isTeaching = true) {
            foreach(var item in this[periods]) {
                item.TimePeriod = TimePeriod;
                item.Type = sectionType;
                item.IsTeaching = isTeaching;
            }
        }

        /// <summary>
        /// 设置节次的启动和结束时间
        /// </summary>
        /// <param name="periods"></param>
        /// <param name="TimePeriod"></param>
        /// <param name="sectionType"></param>
        public void SetSectionTimeSpan(int period, TimeSpan start, TimeSpan end) {
            foreach(var item in this[period]) {
                item.Start = start;
                item.End = end;
            }
        }

        /// <summary>
        /// 设置节次的名字
        /// </summary>
        /// <param name="period"></param>
        /// <param name="name"></param>
        public void SetSectionName(int period, string name) {
            foreach(var item in this[period]) {
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
        public void SetColumnSpan(DayOfWeek day, int period, int columnSpan) {
            int startDay = LayoutOfWeek.IndexOf(day);
            int span = (int)startDay + columnSpan - 1;
            Section section = this[day, period];
            IEnumerable<Section> Sections = this[period, day, columnSpan];
            if(Sections.Any(it => (it.ColSpan > 1 || it.RowSpan > 1) && it.LinkTo != null)) {
                return;
            }
            section.ColSpan = columnSpan;
            for(int i = (int)startDay + 1; i <= span; i++) {
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
        public void SetRowSpan(DayOfWeek day, int period, int rowSpan) {
            int span = period + rowSpan - 1;
            Section section = this[day, period];
            IEnumerable<Section> Sections = this[day, period, rowSpan];
            if(Sections.Any(it => (it.ColSpan > 1 || it.RowSpan > 1) && it.LinkTo != null)) {
                return;
            }
            section.RowSpan = rowSpan;
            for(int i = period + 1; i <= span; i++) {
                Section item = this[i, period];
                item.LinkTo = section;
            }
        }

        /// <summary>
        /// 获取早自习的节次数量
        /// </summary>
        /// <returns></returns>
        public int GetMorningStudyCount() {
            return Sections.Count(s => s.Type == SectionType.MorningStudy);
        }

        /// <summary>
        /// 获取正课授课的节次数量
        /// </summary>
        /// <returns></returns>
        public int GetRegularClassCount() {
            return Sections.Count(s => s.Type == SectionType.RegularClass);
        }

        /// <summary>
        /// 获取晚自习的节次数量
        /// </summary>
        /// <returns></returns>
        public int GetEveningStudyCount() {
            return Sections.Count(s => s.Type == SectionType.EveningStudy);
        }

        /// <summary>
        /// 添加节次到课表
        /// </summary>
        /// <param name="section"></param>
        public void AddSection(Section section) {
            Sections.Add(section);
        }

        /// <summary>
        /// 向节次中安排内容
        /// </summary>
        /// <param name="code"></param>
        /// <param name="content"></param>
        public void AddSectionContent(string code, SectionContent content) {
            this[code].AddSectionContent(content);
        }

        /// <summary>
        /// 验证整个表格中可以放置的位置(抬起验证)
        /// </summary>
        /// <param name="sourceCode">要移动的节次代码</param>
        /// <returns>返回可以放置的课表，或空值</returns>
        public async Task<ClassSchedule?> VerifyExchangeSessionsAsync(string sourceCode) {
            // 备份当前课表
            ClassSchedule? schedule = JsonConvert.DeserializeObject<ClassSchedule>(JsonConvert.SerializeObject(this));
            if(schedule == null || !schedule.Sections.Any()) {
                return null;
            }

            // 当前要移动的节次
            Section? sourceSection = this[sourceCode];
            if(sourceSection == null) {
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
            await VerifyUsableLocations(schedule, sourceSection);
            return schedule;
        }

        /// <summary>
        /// 验证课表中可用的位置
        /// 判断可以移动到的位置
        // 1. 未启用的全部设置为灰(默认状态，无需验证)，不可以放置
        // 2. 已经禁用或者锁定的的节次(默认状态，无需验证), 不可以放置
        // 3. 空白的相同类型的都可以放置(默认状态，无需验证)
        // 4. 只能节次类型相同的放置，不同类型的设置为禁用状态
        // 5. 验证规则通过的可以放置
        //    规则：
        //    1. 源节次占格子必须和目标节次占格子相同
        //    2. 源节次放置到目标节次位置必须满足规则(只能排、不能排、不相邻等规则)
        /// </summary>
        /// <param name="schedule"></param>
        private async Task VerifyUsableLocations(ClassSchedule schedule, Section sourceSection) {
            //foreach(var targetSection in schedule.Sections) {
            await Parallel.ForEachAsync(schedule.Sections.AsParallel().AsOrdered(), async (targetSection, cancellationToken) => {
                // 规则1: 未启用的节次直接设置为禁用
                if(targetSection.Status != SectionStatus.Normal ||
                targetSection.Type != sourceSection.Type) {
                    targetSection.Status = SectionStatus.Disable;
                    return;
                }

                // 规则5: 验证具体规则是否可以放置
                bool canPlace = sourceSection.ColSpan == targetSection.ColSpan &&
                                sourceSection.RowSpan == targetSection.RowSpan &&
                                !targetSection.Contents.Any(x =>
                                    x.Content?.Type == RuleType.CanOnlyArrange ||
                                    x.Content?.Type == RuleType.CannotBeArranged ||
                                    x.Content?.Type == RuleType.ConsecutiveClasses);
                targetSection.Status = canPlace ? SectionStatus.Normal : SectionStatus.Disable;
                await Task.CompletedTask;
            });
        }

        /// <summary>
        /// 交换节次
        ///     既是交换两个节次的内容
        /// </summary>
        /// <param name="sourceCode">源节次代码</param>
        /// <param name="targetCode">目标节次代码</param>
        public async Task<Tuple<bool, string>> ExchangeSessionsAsync(string sourceCode, string targetCode) {
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
        public async Task<Tuple<bool, string>> ExchangeSessionsAsync(Section? sourceSection, Section? targetSection) {
            if(sourceSection == null) {
                return new Tuple<bool, string>(false, "源节次为空不能交换");
            }
            if(targetSection == null) {
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

        #region 获取无冲突的可用节次

        /// <summary>
        /// 获取可用的节次
        /// </summary>
        /// <param name="course">规则</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Section? GetAvailableSections(IRule rule, SectionType regularClass, List<IConstraint> constraint) {
            if(rule == null) {
                return null;
            }
            switch(rule.Type) {
                case RuleType.ConsecutiveClasses: // 连续课，只能取相同时段连续节次，中间无打断
                    return GetAvailableConsecutiveClassesSections(rule, regularClass, constraint);

                case RuleType.None: // 无规则约束，在所有可用的节次中都可以分配
                case RuleType.SingleOrBiweekly:  // 单双周，在所有可用的节次中都可以分配
                case RuleType.AlternatePolling:  // 轮巡，在所有可用的节次中都可以分配
                case RuleType.JointClassTeaching:  // 合班，在所有可用的节次中都可以分配
                case RuleType.CentralizedLessonPreparation:  // 集中备课，只能取指定星期的节次
                default:
                    return GetAvailableOtherSections(rule, regularClass, constraint);
            }
        }

        /// <summary>
        /// 其它规则约束的可用的节次
        /// </summary>
        /// <param name="regularClass"></param>
        /// <returns></returns>
        private Section? GetAvailableOtherSections(IRule rule, SectionType regularClass, List<IConstraint> constraint) {
            // 优先获取约束中只能排的节次
            Section? section = null;
            IConstraint? canOnlyArrangeRule = constraint.FirstOrDefault(r =>
                                            r.Code.Contains(rule.Code) &&
                                            r.Type == RuleType.CanOnlyArrange);
            if(canOnlyArrangeRule != null) {
                section = this.Sections.FirstOrDefault(s =>
                                s.IsValidPair(regularClass) &&
                                s.Day == canOnlyArrangeRule.Location?.Item1 &&
                                s.Period == canOnlyArrangeRule.Location?.Item2);
                if(section != null) {
                    return section; // 找到了符合条件的节次}
                }
            }
            var availableSections = this.Sections.Where(s => s.IsValidPair(regularClass)).ToList();
            if(availableSections.Count() == 0)
                return null; // 如果没有可用的节次
            Random random = new Random();
            int count = availableSections.Count();
            do {
                int index = random.Next(count); // 随机获取索引
                section = availableSections.ElementAt(index); // 返回随机选择的节次
                availableSections.Remove(section); // 移除已选择的节次
                count--;
            }
            while(count > 0 &&
                  CanAssign(section, rule, constraint) is Tuple<bool, string> canAssign &&
                  canAssign.Item1);
            return section; // 无可用节次
        }

        /// <summary>
        /// 获取连堂课的可用的节次
        /// </summary>
        /// <returns></returns>
        private Section? GetAvailableConsecutiveClassesSections(IRule rule, SectionType regularClass, List<IConstraint> constraint) {
            var availableContinuousSections = this.Sections.GroupBy(d => d.Day) // 根据星期分组
                                                           .SelectMany(t => t.GroupBy(s => s.TimePeriod) // 根据时间段分组
                                                               .SelectMany(g => g.Zip(g.Skip(1), (first, second) => new { first, second }) // 使用 Zip 结合相邻元素
                                                                   .Where(pair => IsValidPair(pair.first, pair.second, regularClass)) // 进行有效性判断
                                                                   .Select(pair => pair.first) // 选择连续的节次
                                                               )
                                                           ).ToList();

            if(!availableContinuousSections.Any())
                return null; // 确保列表不为空
            // 需要优先拿约束中只能排的节次
            Section? section = null;
            Random random = new Random();
            int count = availableContinuousSections.Count();

            do {
                int it_index = random.Next(count); // 随机获取索引
                section = availableContinuousSections.ElementAt(it_index); // 返回随机选择的节次
                availableContinuousSections.RemoveAt(it_index); // 移除已选择的节次
                count--;
            }
            while(count > 0 &&
                  CanAssign(section, rule, constraint) is Tuple<bool, string> canAssign &&
                  canAssign.Item1);
            return section; // 无可用节次
        }

        #endregion 获取无冲突的可用节次

        /// <summary>
        /// 判断两个节次是否有效
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="regularClass"></param>
        /// <returns></returns>
        private bool IsValidPair(Section first, Section second, SectionType regularClass) {
            return first.IsValidPair(regularClass) &&
                   second.IsValidPair(regularClass);
        }

        /// <summary>
        /// 判断是否可以分配课程到节次
        /// </summary>
        /// <param name="course"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        //public bool CanAssign(Section section, IRule course, List<IRule>? constraint) {
        //    return section != null &&  !section.IsMergeCell &&  section.LinkTo == null &&
        //           !HasCourseConflict(section, course, constraint);
        //}
        /// <summary>
        /// 判断是否可以分配课程到节次,true:可以分配，false:不可以分配
        /// </summary>
        /// <param name="section"></param>
        /// <param name="course"></param>
        /// <param name="constraint"></param>
        /// <returns></returns>
        public Tuple<bool, string> CanAssign(Section? section, IRule course, List<IConstraint>? constraint) {
            if(section == null) {
                return new Tuple<bool, string>(false, "没有获取到可用节次");
            }
            if(!section.IsValidPair(course.RestrictType)) {
                return new Tuple<bool, string>(false, "节次类型不匹配");
            }
            return HasCourseConflict(section, course, constraint);
        }

        /// <summary>
        /// 判断是否有课程冲突
        ///     有冲突返回 true
        ///     没有冲突返回 false
        /// </summary>
        /// <param name="section"></param>
        /// <param name="course"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Tuple<bool, string> HasCourseConflict(Section? section, IRule course, List<IConstraint>? constraint) {
            if(section == null) {
                return new Tuple<bool, string>(false, "没有获取到可用节次");
            }
            if(constraint == null || !constraint.Any()) {
                return Tuple.Create(false, "没有约束"); // 没有约束直接返回false
            }
            foreach(var rule in constraint) {
                Tuple<bool, string> tuple = VerifyCourseConflict(section, course, rule);
                if(tuple.Item1) {
                    return tuple;
                }
            }
            return Tuple.Create(false, "没有冲突规则");
        }

        /// <summary>
        /// 验证课程冲突
        /// </summary>
        /// <param name="section"></param>
        /// <param name="course"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        private Tuple<bool, string> VerifyCourseConflict(Section section, IRule course, IConstraint rule) {
            switch(rule.Type) {
                case RuleType.CanOnlyArrange: // 只能排课，
                    return VerifyCanOnlyArrange(section, course, rule);

                case RuleType.CannotBeArranged: // 不能排课，
                    return VerifyCannotBeArranged(section, course, rule);

                case RuleType.CentralizedLessonPreparation: // 集中备课，教师教授的课程不能排
                    return VerifyCentralizedLessonPreparation(section, course, rule);

                case RuleType.CoursesAreNotAdjacent:  // 课程不能相邻
                    return VerifyCoursesAreNotAdjacent(section, course, rule);

                case RuleType.Mutex:  // 互斥课，
                default:
                    return Tuple.Create(false, "没有冲突规则");
            }
        }

        /// <summary>
        /// 验证只能排课规则
        /// </summary>
        /// <param name="section"></param>
        /// <param name="course"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private Tuple<bool, string> VerifyCanOnlyArrange(Section section, IRule course, IConstraint rule) {
            CanOnlyArrange canOnlyArranged = (CanOnlyArrange)rule;
            if(!canOnlyArranged.Code.Contains(course.Code) &&
               canOnlyArranged.Location?.Item1 == section.Day &&
               canOnlyArranged.Location?.Item2 == section.Period) {
                return Tuple.Create(true, $"只能排{canOnlyArranged.ClassCourse.DisplayName}");
            }
            return Tuple.Create(false, "没有冲突规则");
        }

        /// <summary>
        /// 验证课程不能相邻规则
        /// </summary>
        /// <param name="section"></param>
        /// <param name="course"></param>
        /// <param name="rule"></param>
        /// <returns>
        ///     true: 课程相邻了
        ///     false: 课程没相邻
        /// </returns>
        private Tuple<bool, string> VerifyCoursesAreNotAdjacent(Section section, IRule course, IConstraint rule) {
            CoursesAreNotAdjacent notAdjacent = (CoursesAreNotAdjacent)rule;
            Section? upSection = this[section.Day, section.Period - 1];   // 前一节次
            Section? downSection = this[section.Day, section.Period + 1]; // 后一节次
            if(upSection != null && upSection.Contents.Any(c => c.Content != null &&
            (c.Content.Code.Contains(course.Code) || notAdjacent.ClassCourses.Any(cc => cc.Code == c.Content.Code)))) {
                return Tuple.Create(true, $"课程{course.DisplayName}不能与{upSection.Day}-{upSection.Name}的课程{upSection.Contents[0].Content?.DisplayName}相邻");
            }
            if(downSection != null && downSection.Contents.Any(c => c.Content != null &&
            (c.Content.Code.Contains(course.Code) || notAdjacent.ClassCourses.Any(cc => cc.Code == c.Content.Code)))) {
                return Tuple.Create(true, $"课程{course.DisplayName}不能与{downSection.Day}-{downSection.Name}放入课程{downSection.Contents[0].Content?.DisplayName}相邻");
            }
            return Tuple.Create(false, "课程无相邻");
        }

        /// <summary>
        /// 验证集中备课规则
        /// </summary>
        /// <param name="section"></param>
        /// <param name="course"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        private Tuple<bool, string> VerifyCentralizedLessonPreparation(Section section, IRule course, IConstraint rule) {
            CentralizedLessonPreparation centralizedLessonPreparation = (CentralizedLessonPreparation)rule;
            if(centralizedLessonPreparation.Location?.Item1 == section.Day &&
                centralizedLessonPreparation.Location?.Item2 == section.Period &&
                centralizedLessonPreparation.ClassCourse.Code == course.Code) {
                return Tuple.Create(true, $"不能排{course.DisplayName}");
            }
            return Tuple.Create(false, "没有冲突规则");
        }

        /// <summary>
        /// 验证不能排课规则
        /// </summary>
        /// <param name="section"></param>
        /// <param name="course"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        private Tuple<bool, string> VerifyCannotBeArranged(Section section, IRule course, IConstraint rule) {
            CannotBeArranged cannotBeArranged = (CannotBeArranged)rule;
            if(cannotBeArranged.Code.Contains(course.Code) &&
                cannotBeArranged.Location?.Item1 == section.Day &&
                cannotBeArranged.Location?.Item2 == section.Period) {
                return Tuple.Create(true, $"不能排{course.DisplayName}");
            }
            return Tuple.Create(false, "没有冲突规则");
        }

        /// <summary>
        /// 撤销已分配的课程
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void RemoveSectionContent(object id) {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 安排只能排课程
        /// </summary>
        /// <param name="courses">需要安排的课程集合</param>
        /// <param name="enumerable">只能排课程集合</param>
        /// <exception cref="NotImplementedException"></exception>
        public void RunCanOnlyArrange(List<IRule> courses, IEnumerable<IConstraint> enumerables) {
            foreach(var enumerable in enumerables) {
                CanOnlyArrange canOnlyArrange = (CanOnlyArrange)enumerable;
                IEnumerable<IRule> theCourses = courses.FindAll(c => c.Code == canOnlyArrange.Code && c.Type == RuleType.None);
                if(theCourses.Any() && canOnlyArrange.Location != null) {
                    DayOfWeek day = canOnlyArrange.Location.Item1;
                    int period = canOnlyArrange.Location.Item2;
                    Section? section = this[day, period];
                    if(section != null) {
                        IRule theCourse = theCourses.First();
                        AddSectionContent(section.Code, new SectionContent(0, theCourse));
                        courses.Remove(theCourse);
                    }
                }
            }
        }

        /// <summary>
        /// 验证需要排课的课时是否超出限制（课程课时必须小于等于节次数量，否则课程不能安排完全）
        /// </summary>
        /// <param name="courses"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool VerifyCourseHours(List<IRule> courses) {
            bool result = true;
            // 验证早自习课时、正课授课课时、晚自习课时
            float morningStudyCount = courses.Select(c => {
                if(c.RestrictType == SectionType.MorningStudy && c.Type != RuleType.ConsecutiveClasses) {
                    return 1;
                }
                else if(c.RestrictType == SectionType.MorningStudy && c.Type == RuleType.ConsecutiveClasses) {
                    return 2;
                }
                return 0;
            }).Sum();
            if(morningStudyCount > GetMorningStudyCount()) {
                return false;
            }
            float regularClassCount = courses.Select(c => {
                if(c.RestrictType == SectionType.RegularClass && c.Type != RuleType.ConsecutiveClasses) {
                    return 1;
                }
                else if(c.RestrictType == SectionType.RegularClass && c.Type == RuleType.ConsecutiveClasses) {
                    return 2;
                }
                return 0;
            }).Sum();
            if(regularClassCount > GetRegularClassCount()) {
                return false;
            }
            float eveningStudyCount = courses.Select(c => {
                if(c.RestrictType == SectionType.EveningStudy && c.Type != RuleType.ConsecutiveClasses) {
                    return 1;
                }
                else if(c.RestrictType == SectionType.EveningStudy && c.Type == RuleType.ConsecutiveClasses) {
                    return 2;
                }
                return 0;
            }).Sum();
            if(eveningStudyCount > GetEveningStudyCount()) {
                return false;
            }
            return result;
        }
    }
}