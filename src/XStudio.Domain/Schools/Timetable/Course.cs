using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using XStudio.Common;

namespace XStudio.Schools.Timetable
{
    [DbDescription("课程")]
    public class Course : AuditedAggregateRoot<Guid>
    {
        [DbDescription("序号")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Order { get; set; } = 0;
        /// <summary>
        /// 课程编号，主要用于快速识别和查询
        /// </summary>
        [DbDescription("编号")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 课程名称
        /// </summary>
        [DbDescription("课程名称")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// "数据有效标识：A 正常 | E 异常 | S 停用 | D 删除
        /// </summary>

        [DbDescription("数据有效标识")]
        public ValidStateType ValidState { get; set; } = ValidStateType.A;
    }
}
