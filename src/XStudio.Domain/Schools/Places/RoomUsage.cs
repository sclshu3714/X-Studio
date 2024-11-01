using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using XStudio.Common;

namespace XStudio.Schools.Places {
    /// <summary>
    /// 房间用途表
    /// </summary>
    [Description("房间用途")]
    public class RoomUsage : AuditedAggregateRoot<Guid> {
        /// <summary>
        /// 用途序号
        /// </summary>
        [Description("序号")]
        public int Order { get; set; } = 0;

        /// <summary>
        /// 用途编号
        /// </summary>
        [Description("编码")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 用途名称
        /// </summary>
        /// <example>
        /// 教室
        /// 办公室
        /// 休息室
        /// 实验室
        /// 体育器材保管室
        /// 食堂
        /// 宿舍
        /// 音乐室
        /// 图书馆
        /// 化学实验室
        /// 演播室
        /// </example>
        [Description("名称")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// "数据有效标识：A 正常 | E 异常 | S 停用 | D 删除
        /// </summary>
        [Description("数据有效标识")]
        public ValidStateType ValidState { get; set; } = ValidStateType.A;
    }
}
