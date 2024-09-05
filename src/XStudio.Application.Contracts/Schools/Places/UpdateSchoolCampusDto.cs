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
    /// 校区
    /// </summary>
    public class UpdateSchoolCampusDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 学校编码
        /// </summary>
        public string SchoolCode { get; set; } = string.Empty;

        /// <summary>
        /// 序号
        /// </summary>
        public long Order { get; set; } = 0;

        /// <summary>
        /// 校区编号
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 校区名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 校址
        /// </summary>
        public string Address { get; set; } = string.Empty;
    }
}
