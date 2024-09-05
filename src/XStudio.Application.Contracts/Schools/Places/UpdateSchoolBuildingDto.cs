using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using XStudio.Common;

namespace XStudio.Schools.Places
{
    /// <summary>
    /// 楼栋
    /// </summary>
    public class UpdateSchoolBuildingDto : AuditedEntityDto<Guid>
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
        public long Order { get; set; } = 0;

        /// <summary>
        /// 楼栋编号
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 楼栋名称
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
