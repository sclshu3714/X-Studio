using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using XStudio.Common;

/*
 默认时段:
        早晨 - Morning
        上午 - Forenoon
        中午 - Noon
        下午 - Afternoon
        晚上 - Evening
 */

namespace XStudio.Schools.Timetable
{
    [DbDescription("时段")]
    public class TimePeriod : AuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 序号
        /// </summary>
        [DbDescription("序号")]
        public int Order { get; set; } = 0;

        /// <summary>
        /// 学校编号
        /// </summary>
        [DbDescription("编码")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 学校名称
        /// </summary>
        [DbDescription("名称")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// "数据有效标识：A 正常 | E 异常 | S 停用 | D 删除
        /// </summary>
        [DbDescription("数据有效标识")]
        public ValidStateType ValidState {  get; set; } = ValidStateType.A;
    }
}
