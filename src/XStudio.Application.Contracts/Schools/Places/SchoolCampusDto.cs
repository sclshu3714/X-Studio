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
    public class SchoolCampusDto : AuditedEntityDto<Guid>
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

        /// <summary>
        /// 属于学校
        /// </summary>
        public virtual SchoolDto? School { get; set; }

        /// <summary>
        /// 包含楼栋
        /// </summary>
        public virtual ICollection<SchoolBuildingDto> Buildings { get; set; } = new List<SchoolBuildingDto>();

        /// <summary>
        /// "数据有效标识：A 正常 | E 异常 | S 停用 | D 删除
        /// </summary>
        public ValidStateType ValidState { get; set; } = ValidStateType.A;
    }
}
