using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace XStudio.Projects
{
    public class ProjectDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 课件名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 课件类型
        /// </summary>
        public ProjectType @Type { get; set; } = ProjectType.Undefined;

        /// <summary>
        /// 获取课件类型字典
        /// </summary>
        public Dictionary<int, string> TypeDic
        {
            get
            {
                var typeDictionary = new Dictionary<int, string>();
                //foreach (ProjectType type in Enum.GetValues(typeof(ProjectType)))
                //{
                //    typeDictionary.Add((int)type, type.ToString());
                //}
                return typeDictionary;
            }
        }
        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// 发布者编号
        /// </summary>
        public Guid AuthorId { get; set; }

        /// <summary>
        /// 发布者名称
        /// </summary>
        public string AuthorName { get; set; } = string.Empty;
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
