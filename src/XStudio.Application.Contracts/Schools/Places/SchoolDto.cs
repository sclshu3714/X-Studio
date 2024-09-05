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
    /// 学校
    /// </summary>
    public class SchoolDto : AuditedEntityDto<Guid>
    {
        public SchoolDto() { }

        /// <summary>
        /// 学校编号
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 学校名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 促销口号，宣传语
        /// </summary>
        public string PromotionSlogan { get; set; } = string.Empty;

        /// <summary>
        /// 学校简介
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 校徽
        /// </summary>
        public string Badge { get; set; } = string.Empty;

        /// <summary>
        /// 校址
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 包含校区
        /// </summary>
        public virtual ICollection<SchoolCampusDto> Campuses { get; set; } = new List<SchoolCampusDto>();

        /// <summary>
        /// "数据有效标识：A 正常 | E 异常 | S 停用 | D 删除
        /// </summary>
        public ValidStateType ValidState { get; set; } = ValidStateType.A;
    }
}
