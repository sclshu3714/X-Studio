using COSXML.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Common.Clouds {
    public class CustomQCloudCredentialProvider : DefaultSessionQCloudCredentialProvider {
        private string tmpSecretId;
        private string tmpSecretKey;
        private string tmpToken;
        private long tmpStartTime;
        private long tmpExpiredTime;

        // 这里假设开始没有密钥，也可以用初始的临时密钥来初始化
        public CustomQCloudCredentialProvider(string tmpSecretId,
                                              string tmpSecretKey,
                                              string tmpToken,
                                              long tmpStartTime,
                                              long tmpExpiredTime)
            : base(tmpSecretId, tmpSecretKey, tmpExpiredTime, tmpToken) {
            this.tmpSecretId = tmpSecretId;
            this.tmpSecretKey = tmpSecretKey;
            this.tmpToken = tmpToken;
            this.tmpStartTime = tmpStartTime;
            this.tmpExpiredTime = tmpExpiredTime;
        }

        public override void Refresh() {
            ////... 首先通过腾讯云请求临时密钥
            // string tmpSecretId = "SECRET_ID"; //"临时密钥 SecretId", 临时密钥生成和使用指引参见 https://cloud.tencent.com/document/product/436/14048
            // string tmpSecretKey = "SECRET_KEY"; //"临时密钥 SecretKey", 临时密钥生成和使用指引参见 https://cloud.tencent.com/document/product/436/14048
            // string tmpToken = "COS_TOKEN"; //"临时密钥 token", 临时密钥生成和使用指引参见 https://cloud.tencent.com/document/product/436/14048
            // long tmpStartTime = 1546860702;//临时密钥有效开始时间，精确到秒
            // long tmpExpiredTime = 1546862502;//临时密钥有效截止时间，精确到秒
            tmpStartTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            tmpExpiredTime = new DateTimeOffset(DateTime.UtcNow.AddSeconds(3600)).ToUnixTimeSeconds();
            // 调用接口更新密钥
            SetQCloudCredential(tmpSecretId, tmpSecretKey, String.Format("{0};{1}", tmpStartTime, tmpExpiredTime), tmpToken);
        }
    }
}
