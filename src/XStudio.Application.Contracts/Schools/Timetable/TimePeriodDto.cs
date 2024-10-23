using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Volo.Abp.Application.Dtos;

namespace XStudio.Schools.Timetable {
    public class TimePeriodDto : AuditedEntityDto<Guid> {
        public int Order { get; set; } = 0;

        /// <summary>
        /// 时段编号
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 学校名称
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateTimePeriodDto : AuditedEntityDto<Guid> {
        public int Order { get; set; } = 0;

        /// <summary>
        /// 时段编号
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 学校名称
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }

    public class CreateTimePeriodDto : AuditedEntityDto<Guid> {
        public int Order { get; set; } = 0;

        /// <summary>
        /// 时段编号
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 学校名称
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
