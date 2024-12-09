using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Common {
    public class TencentCloudOption {

        /// <summary>
        /// 是否启用腾讯云
        /// </summary>
        public bool IsEnable { get; set; } = true;

        /// <summary>
        /// 是否加密
        /// </summary>
        public bool IsEncrypt { get; set; } = true;


        /// <summary>
        /// 腾讯云SecretId
        /// </summary>
        public string SecretId { get; set; } = string.Empty; // 腾讯云SecretId

        /// <summary>
        /// 腾讯云SecretKey
        /// </summary>
        public string SecretKey { get; set; } = string.Empty; // 腾讯云SecretKey

        /// <summary>
        /// 腾讯云区域
        /// </summary>
        public string Region { get; set; } = string.Empty; // 腾讯云区域

        /// <summary>
        /// 腾讯云存储桶
        /// </summary>
        public string Bucket { get; set; } = string.Empty;

        public int DurationSeconds { get; set; } = 3600; // 签名有效期

        /// <summary>
        /// 腾讯云域名
        /// </summary>
        public string Domain { get; set; } = string.Empty; // 腾讯云域名
    }
}
