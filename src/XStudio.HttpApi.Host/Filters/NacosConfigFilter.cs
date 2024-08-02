﻿using System;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Nacos.V2.Config.Abst;
using Nacos.V2;
using Nacos.V2.Config;

namespace XStudio.Filters
{
    public class NacosConfigFilter : IConfigFilter
    {
        public void DoFilter(IConfigRequest request, IConfigResponse response, IConfigFilterChain filterChain)
        {
            if (request != null)
            {
                var raw_content = request.GetParameter(ConfigConstants.CONTENT);

                // 部分配置加密后的 content
                var content = ReplaceJsonNode((string)raw_content, true);

                // after encrypt the content, don't forget to update the request!!!
                request.PutParameter(ConfigConstants.ENCRYPTED_DATA_KEY, "");
                request.PutParameter(ConfigConstants.CONTENT, content);
            }

            if (response != null)
            {
                var resp_content = response.GetParameter(ConfigConstants.CONTENT);
                var resp_encryptedDataKey = response.GetParameter(ConfigConstants.ENCRYPTED_DATA_KEY);

                // nacos 2.0.2 still do not return the encryptedDataKey yet
                // but we can use a const key here.
                // after nacos server return the encryptedDataKey, we can keep one dataid with one encryptedDataKey
                var encryptedDataKey = (resp_encryptedDataKey == null || string.IsNullOrWhiteSpace((string)resp_encryptedDataKey)) ? string.Empty : (string)resp_encryptedDataKey;

                var content = ReplaceJsonNode((string)resp_content, false);
                response.PutParameter(ConfigConstants.CONTENT, content);
            }
        }

        private string ReplaceJsonNode(string src, bool isEnc = true)
        {
            var jObj = JObject.Parse(src);

            if (_jsonPaths != null)
            {
                foreach (var item in _jsonPaths)
                {
                    var t = jObj.SelectToken(item);

                    if (t != null)
                    {
                        var r = t.ToString();

                        // 加解密，示例用 base64
                        var newToken = isEnc
                            ? Convert.ToBase64String(Encoding.UTF8.GetBytes(r))
                            : Encoding.UTF8.GetString(Convert.FromBase64String(r));

                        t.Replace(newToken);
                    }
                }
            }

            Console.WriteLine(jObj.ToString());

            return jObj.ToString();
        }

        public string GetFilterName() => nameof(NacosConfigFilter);

        public int GetOrder() => 1;

        private List<string>? _jsonPaths;

        public void Init(NacosSdkOptions options)
        {
            // 从拓展信息里面获取需要加密的 json path
            // 这里只是示例，根据具体情况调整成自己合适的！！！！
            var extInfo = JObject.Parse(options.ConfigFilterExtInfo);

            if (extInfo.ContainsKey("JsonPaths"))
            {
                _jsonPaths = extInfo?.GetValue("JsonPaths")?.ToObject<List<string>>();
            }

            Console.WriteLine("Assemblies = " + string.Join(",", options.ConfigFilterAssemblies));
            Console.WriteLine("Ext Info = " + string.Join(",", options.ConfigFilterExtInfo));
            Console.WriteLine("Init");
        }
    }
}
