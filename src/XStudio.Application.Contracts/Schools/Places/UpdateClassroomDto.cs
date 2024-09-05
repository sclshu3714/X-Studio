using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using XStudio.Common;

namespace XStudio.Schools.Places
{
    /// <summary>
    /// 教室
    /// </summary>
    public class UpdateClassroomDto : AuditedEntityDto<Guid>
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
        /// 楼栋编码
        /// </summary>
        public string BuildingCode { get; set; } = string.Empty;

        /// <summary>
        /// 楼层编码
        /// </summary>
        public string FloorCode { get; set; } = string.Empty;

        /// <summary>
        /// 序号
        /// </summary>
        public long Order { get; set; } = 0;

        /// <summary>
        /// 教室编号
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 教室名称
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
