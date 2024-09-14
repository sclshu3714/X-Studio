using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Nacos.V2;
using Volo.Abp.DependencyInjection;

namespace XStudio.Common.Nacos
{
    public class NacosJsonHelper
    {
        /// <summary>
        /// yml 转 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T? DeserializeObject<T>(string value)
        {
            var deserializer = new DeserializerBuilder().Build();
            var serializer = new SerializerBuilder().JsonCompatible().Build();
            var json = serializer.Serialize(deserializer.Deserialize(new StringReader(value)));
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// yml 转 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DeserializeString(string value)
        {
            var deserializer = new DeserializerBuilder().Build();
            var serializer = new SerializerBuilder().JsonCompatible().Build();
            var json = serializer.Serialize(deserializer.Deserialize(new StringReader(value)));
            return json;
        }

        /// <summary>
        ///  yml 获取字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="key">读取数据节点（多个节点用:隔开）</param>
        /// <returns></returns>
        public static string? DeserializeString(string value, string key)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            var deserializer = new DeserializerBuilder().Build();
            var serializer = new SerializerBuilder().JsonCompatible().Build();
            var json = serializer.Serialize(deserializer.Deserialize(new StringReader(value)));

            JToken? jToken = JObject.Parse(json);
            foreach (var item in key.Split(':'))
            {
                if (jToken == null)
                {
                    return null;
                }
                jToken = jToken[item];
            }
            return jToken?.ToString();
        }

        /// <summary>
        ///  yml 转 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="key">读取数据节点（多个节点用:隔开）</param>
        /// <returns></returns>
        public static T? DeserializeObject<T>(string value, string key)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }
            try
            {
                var deserializer = new DeserializerBuilder().Build();
                var serializer = new SerializerBuilder().JsonCompatible().Build();
                var json = serializer.Serialize(deserializer.Deserialize(new StringReader(value)));

                JToken jTokens = JObject.Parse(json);
                if (jTokens == null)
                {
                    return default;
                }
                JToken? jToken = null;
                foreach (var item in key.Split(':'))
                {
                    if (!jTokens.Contains(item))
                        break;
                    jToken = jTokens[item];
                }
                if (jToken != null)
                {
                    return jToken.ToObject<T>();
                }
                return default;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
