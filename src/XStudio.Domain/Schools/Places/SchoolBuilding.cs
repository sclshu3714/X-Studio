using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using XStudio.Common;

namespace XStudio.Schools.Places
{
    [Description("楼栋")]
    public class SchoolBuilding : AuditedAggregateRoot<Guid>
    {

        /// <summary>
        /// 学校编码
        /// </summary>
        public string SchoolCode { get; set; } = string.Empty;

        /// <summary>
        /// 校区编码
        /// </summary>
        public string SchoolCampusCode { get; set; } = string.Empty;

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
        /// 属于学校
        /// </summary>
        public virtual School? School { get; set; }

        /// <summary>
        /// 属于校区
        /// </summary>
        public virtual SchoolCampus? Campus { get; set; }

        /// <summary>
        /// 包含楼层
        /// </summary>
        public virtual ICollection<BuildingFloor> Floors { get; set; } = new List<BuildingFloor>();

        /// <summary>
        /// "数据有效标识：A 正常 | E 异常 | S 停用 | D 删除
        /// </summary>

        [Description("数据有效标识")]
        public ValidStateType ValidState { get; set; } = ValidStateType.A;
    }
}
