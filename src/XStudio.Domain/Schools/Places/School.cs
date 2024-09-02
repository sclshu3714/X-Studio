using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using XStudio.Common;

namespace XStudio.Schools.Places
{
    [DbDescription("学校")]
    public class School : AuditedAggregateRoot<Guid>
    {
        public School() { }

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
        /// 促销口号，宣传语
        /// </summary>
        [DbDescription("宣传语")]
        public string PromotionSlogan { get; set; } = string.Empty;

        /// <summary>
        /// 学校简介
        /// </summary>
        [DbDescription("简介")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 校徽
        /// </summary>
        [DbDescription("校徽")]
        public string Badge { get; set; } = string.Empty;

        /// <summary>
        /// 校址
        /// </summary>
        [DbDescription("校址")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 包含校区
        /// </summary>
        public virtual ICollection<SchoolCampus> Campuses { get; set; } = new List<SchoolCampus>();
    }
}
