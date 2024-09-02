using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using XStudio.Common;

namespace XStudio.Schools.Places
{
    /// <summary>
    /// 校区
    /// </summary>
    [DbDescription("校区")]
    public class SchoolCampus : AuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 学校编码
        /// </summary>
        public string SchoolCode { get; set; } = string.Empty;

        /// <summary>
        /// 序号
        /// </summary>
        [DbDescription("序号")]
        public long Index { get; set; } = 0;

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
        /// 校址
        /// </summary>
        [DbDescription("校址")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 属于学校
        /// </summary>
        public virtual School School { get; set; }

        /// <summary>
        /// 包含楼栋
        /// </summary>
        public virtual ICollection<SchoolBuilding> Buildings { get; set; } = new List<SchoolBuilding>();
    }
}
