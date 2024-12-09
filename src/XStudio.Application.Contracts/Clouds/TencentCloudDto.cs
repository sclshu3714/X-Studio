using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Clouds {
    public class TencentCloudDto {
        public Credentials Credentials { get; set; } = new Credentials();
        public string requestId { get; set; } = string.Empty;
        public string expiration { get; set; } = string.Empty;
        public long startTime { get; set; }
        public long expiredTime { get; set; }
    }

    public class Credentials {

        /// <summary>
        /// 临时密钥Id
        /// </summary>
        public string tmpSecretId { get; set; } = string.Empty;

        /// <summary>
        /// 临时密钥Key
        /// </summary>
        public string tmpSecretKey { get; set; } = string.Empty;

        /// <summary>
        /// 授权Token
        /// </summary>
        public string sessionToken { get; set; } = string.Empty;

        /// <summary>
        /// 令牌
        /// </summary>
        public string token { get; set; } = string.Empty;
        public Credentials() { }
    }
}
