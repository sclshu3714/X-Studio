using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Clouds {
    [Serializable]
    public class TencentCloudCredentialsDto {

        /// <summary>
        /// 腾讯云临时密钥
        /// </summary>
        [JsonProperty("credentials")]
        public Credentials Credentials { get; set; } = new Credentials();

        /// <summary>
        /// 请求Id
        /// </summary>

        [JsonProperty("requestId")]
        public string RequestId { get; set; } = string.Empty;

        /// <summary>
        /// 过期时间
        /// </summary>

        [JsonProperty("expiration")]
        public string Expiration { get; set; } = string.Empty;

        /// <summary>
        /// 开始时间
        /// </summary>

        [JsonProperty("startTime")]
        public long StartTime { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>

        [JsonProperty("expiredTime")]
        public long ExpiredTime { get; set; }

        /// <summary>
        /// 腾讯云区域
        /// </summary>
        [JsonProperty("region")]
        public string Region { get; set; } = string.Empty; // 腾讯云区域

        /// <summary>
        /// 腾讯云存储桶
        /// </summary>
        [JsonProperty("bucket")]
        public string Bucket { get; set; } = string.Empty;
    }
    [Serializable]
    public class Credentials {

        /// <summary>
        /// 临时密钥Id
        /// </summary>
        [JsonProperty("tmpSecretId")]
        public string TmpSecretId { get; set; } = string.Empty;

        /// <summary>
        /// 临时密钥Key
        /// </summary>
        [JsonProperty("tmpSecretKey")]
        public string TmpSecretKey { get; set; } = string.Empty;

        /// <summary>
        /// 授权Token
        /// </summary>
        [JsonProperty("sessionToken")]
        public string SessionToken { get; set; } = string.Empty;

        /// <summary>
        /// 令牌
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; } = string.Empty;
    }
}
