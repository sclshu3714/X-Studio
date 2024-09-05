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
    /// 楼层
    /// </summary>
    public class BuildingFloorDto : AuditedEntityDto<Guid>
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
        /// 序号
        /// </summary>
        public long Order { get; set; } = 0;

        /// <summary>
        /// 楼层编号
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 楼层名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 属于学校
        /// </summary>
        public virtual SchoolDto? School { get; set; }

        /// <summary>
        /// 属于校区
        /// </summary>
        public virtual SchoolCampusDto? Campus { get; set; }

        /// <summary>
        /// 属于楼栋
        /// </summary>
        public virtual SchoolBuildingDto? Building { get; set; }
        

        /// <summary>
        /// 包含教室
        /// </summary>
        public virtual ICollection<ClassroomDto> Classrooms { get; set; } = new List<ClassroomDto>();

        /// <summary>
        /// "数据有效标识：A 正常 | E 异常 | S 停用 | D 删除
        /// </summary>
        public ValidStateType ValidState { get; set; } = ValidStateType.A;
    }
}
