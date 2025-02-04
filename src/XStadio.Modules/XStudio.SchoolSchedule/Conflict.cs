using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.SchoolSchedule {
    /// <summary>
    /// 冲突
    /// </summary>
    public class Conflict {

        /// <summary>
        /// 冲突ID
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 冲突内容
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 冲突类型
        /// </summary>
        public string @Type { get; set; } = string.Empty;

        /// <summary>
        /// 冲突节次Code(在哪个节次中发生冲突)
        /// </summary>
        public string SectionCode { get; set; } = string.Empty;
    }
}
