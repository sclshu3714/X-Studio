using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using XStudio.Common;

namespace XStudio.Schools.Places
{
    [DbDescription("教室")]
    public class Classroom : AuditedAggregateRoot<Guid>
    {
        [DbDescription("学校编码")]
        public string SchoolCode { get; set; } = string.Empty;

        [DbDescription("校区编码")]
        public string SchoolCampusCode { get; set; } = string.Empty;

        [DbDescription("楼栋编码")]
        public string BuildingCode { get; set; } = string.Empty;

        [DbDescription("楼层编码")]
        public string FloorCode { get; set; } = string.Empty;

        /// <summary>
        /// 序号
        /// </summary>
        [DbDescription("序号")]
        public long Order { get; set; } = 0;

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
        /// 属于学校
        /// </summary>
        public virtual School? School { get; set; }

        /// <summary>
        /// 属于校区
        /// </summary>
        public virtual SchoolCampus? Campus { get; set; }

        /// <summary>
        /// 属于楼栋
        /// </summary>
        public virtual SchoolBuilding? Building { get; set; }
        
        /// <summary>
        /// 属于楼层
        /// </summary>
        public virtual BuildingFloor? Floor { get; set; }

        /// <summary>
        /// "数据有效标识：A 正常 | E 异常 | S 停用 | D 删除
        /// </summary>

        [DbDescription("数据有效标识")]
        public ValidStateType ValidState { get; set; } = ValidStateType.A;
    }
}
