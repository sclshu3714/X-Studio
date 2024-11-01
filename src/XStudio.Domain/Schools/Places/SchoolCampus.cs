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
    /// <summary>
    /// 校区
    /// </summary>
    [Description("校区")]
    public class SchoolCampus : AuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 学校编码
        /// </summary>
        public string SchoolCode { get; set; } = string.Empty;

        /// <summary>
        /// 序号
        /// </summary>
        [Description("序号")]
        public long Order { get; set; } = 0;

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
        /// 校址
        /// </summary>
        [Description("校址")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 属于学校
        /// </summary>
        public virtual School? School { get; set; }

        /// <summary>
        /// 包含楼栋
        /// </summary>
        public virtual ICollection<SchoolBuilding> Buildings { get; set; } = new List<SchoolBuilding>();

        /// <summary>
        /// "数据有效标识：A 正常 | E 异常 | S 停用 | D 删除
        /// </summary>

        [Description("数据有效标识")]
        public ValidStateType ValidState { get; set; } = ValidStateType.A;
    }
}
