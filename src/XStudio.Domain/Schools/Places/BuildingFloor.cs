using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using XStudio.Common;

namespace XStudio.Schools.Places
{
    [DbDescription("楼层")]
    public class BuildingFloor : AuditedAggregateRoot<Guid>
    {
        [DbDescription("学校编码")]
        public string SchoolCode { get; set; } = string.Empty;

        [DbDescription("校区编码")]
        public string SchoolCampusCode { get; set; } = string.Empty;

        [DbDescription("楼栋编码")]
        public string BuildingCode { get; set; } = string.Empty;

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
        /// 属于楼栋
        /// </summary>
        public virtual SchoolBuilding Building { get; set; }

        /// <summary>
        /// 包含教室
        /// </summary>
        public virtual ICollection<Classroom> Classrooms { get; set; } = new List<Classroom>();
    }
}
