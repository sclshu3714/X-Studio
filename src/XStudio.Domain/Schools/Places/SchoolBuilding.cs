using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XStudio.Common;

namespace XStudio.Schools.Places
{
    [DbDescription("楼栋")]
    public class SchoolBuilding
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
        [DbDescription("序号")]
        public long Index { get; set; } = 0;

        /// <summary>
        /// 学校编号
        /// </summary>
        [DbDescription("编码")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 学校名称
        /// </summary>
        [DbDescription("名称")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 属于学校
        /// </summary>
        public virtual School? School { get; set; }

        /// <summary>
        /// 属于校区
        /// </summary>
        public virtual SchoolCampus? Campus { get; set; }

        /// <summary>
        /// 包含楼层
        /// </summary>
        public virtual ICollection<BuildingFloor> Floors { get; set; } = new List<BuildingFloor>();
    }
}
