using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Clouds {
    [Serializable]
    public class UploadFileDto {
        [JsonProperty("form")]
        public IFormFile? Form { get; set; }
        /// <summary>
        /// 记录文件本来的名称(代后缀)
        /// </summary>
        [JsonProperty("fileName")]
        public string? FileName { get; set; } = string.Empty;

        /// <summary>
        /// 记录文件类型（文件后缀-带 "." 符号.）
        /// </summary>
        [JsonProperty("fileType")]
        public string? FileType { get; set; } = string.Empty;

        /// <summary>
        /// 记录文件在云存储中的唯一标识符
        /// </summary>
        [JsonProperty("fileKey")]
        public string FileKey { get; set; } = string.Empty;

        /// <summary>
        /// 记录文件所属的区域
        /// </summary>
        [JsonProperty("region")]
        public string Region { get; set; } = "ap-chengdu";

        /// <summary>
        /// 记录文件存储到的存储桶
        /// </summary>
        [JsonProperty("bucket")]
        public string Bucket { get; set; } = "jf-aic-prod-1304449501";
    }
}
