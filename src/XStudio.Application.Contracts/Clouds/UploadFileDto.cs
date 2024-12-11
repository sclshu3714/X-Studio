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

    public class UploadFileResultDto {
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
        /// 记录文件请求地址
        /// </summary>
        [JsonProperty("fileUrl")]
        public string FileUrl { get; set; } = string.Empty;

        /// <summary>
        ///  腾讯云对象的URL(当对象 ACL 属性设置为“公有读”时，可以通过以下 SDK 接口生成的 URL 直接访问对象（仅支持生成 COS 默认源站域名的 URL）。)
        /// </summary>
        [JsonProperty("objectUrl")]
        public string? ObjectUrl { get; set; } = string.Empty;

        /// <summary>
        /// 预签名 URL, 有签名的下载预览地址， 有效期默认为12小时，在配置文件中可修改
        /// </summary>
        [JsonProperty("preSignDownloadUrl")]
        public string? PreSignDownloadUrl { get; set; } = string.Empty;

        /// <summary>
        /// 记录上传是否成功
        /// </summary>
        [JsonProperty("success")]
        public bool Success { get; set; } = false;

        /// <summary>
        /// 记录上传失败的错误信息
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 记录文件大小
        /// </summary>
        [JsonProperty("fileSize")]
        public long FileSize { get; set; }
    }
}
