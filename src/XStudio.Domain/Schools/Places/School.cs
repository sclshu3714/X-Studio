using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using XStudio.Common;

namespace XStudio.Schools.Places
{
    [Description("学校")]
    public class School : AuditedAggregateRoot<Guid>
    {
        public School() { }

        /// <summary>
        /// 学校编号
        /// </summary>
        [Description("编码")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 学校名称
        /// </summary>
        [Description("名称")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 促销口号，宣传语
        /// </summary>
        [Description("宣传语")]
        public string PromotionSlogan { get; set; } = string.Empty;

        /// <summary>
        /// 学校简介
        /// </summary>
        [Description("简介")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 校徽
        /// </summary>
        [Description("校徽")]
        public string Badge { get; set; } = string.Empty;

        /// <summary>
        /// 校址
        /// </summary>
        [Description("校址")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 包含校区
        /// </summary>
        public virtual ICollection<SchoolCampus> Campuses { get; set; } = new List<SchoolCampus>();

        /// <summary>
        /// "数据有效标识：A 正常 | E 异常 | S 停用 | D 删除
        /// </summary>

        [Description("数据有效标识")]
        public ValidStateType ValidState { get; set; } = ValidStateType.A;
    }
}
