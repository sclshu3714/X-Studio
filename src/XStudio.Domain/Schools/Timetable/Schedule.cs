using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using XStudio.Common;

namespace XStudio.Schools.Timetable
{
    [DbDescription("节次方案表")]
    public class Schedule : AuditedAggregateRoot<Guid>
    {
        [DbDescription("序号")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Order { get; set; } = 0;
        /// <summary>
        /// 节次表编号，主要用于快速识别和查询
        /// </summary>
        [DbDescription("编号")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 节次表名称
        /// </summary>
        [DbDescription("节次方案名称")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 包含的节信息
        /// </summary>
        [DbDescription("节次表中包含的节次")]
        public virtual List<Section> Sections { get; set; } = new List<Section>();

        /// <summary>
        /// 默认周一 - 周日,当然也可以控制周日 - 周六,周六 - 周五等
        /// </summary>
        [DbDescription("布局节次表")]
        public List<DayOfWeek> LayoutOfWeek { get; set; } = new List<DayOfWeek>() { 
             DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
             DayOfWeek.Friday, DayOfWeek.Saturday,DayOfWeek.Sunday
        };

        /// <summary>
        /// "数据有效标识：A 正常 | E 异常 | S 停用 | D 删除
        /// </summary>

        [DbDescription("数据有效标识")]
        public ValidStateType ValidState { get; set; } = ValidStateType.A;
    }
}
