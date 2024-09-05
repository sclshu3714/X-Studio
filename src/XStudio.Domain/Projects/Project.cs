using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using XStudio.Common;

namespace XStudio.Projects
{
    [DbDescription("项目表")]
    public class Project : AuditedAggregateRoot<Guid>
    {
        public Project() { }
        /// <summary>
        /// 课件名称
        /// </summary>
        [DbDescription("项目名称")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 课件类型
        /// </summary>
        public ProjectType @Type { get; set; } = ProjectType.Undefined;

        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// 发布者
        /// </summary>
        public Guid AuthorId { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = string.Empty;
    
               /// <summary>
        /// "数据有效标识：A 正常 | E 异常 | S 停用 | D 删除
        /// </summary>

        [DbDescription("数据有效标识")]
        public ValidStateType ValidState { get; set; } = ValidStateType.A;
    }
}
