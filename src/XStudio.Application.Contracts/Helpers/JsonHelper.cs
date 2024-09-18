using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.IO;
using System;
using XStudio.Converters;
using Microsoft.IdentityModel.Logging;
using Serilog;

namespace XStudio.Helpers
{
    /// <summary>
    /// json工具类
    /// </summary>
    public static class JsonHelper
    {
        private static string _loggerHead = "JsonHelper";
        private static JsonSerializerSettings _jsonSettings;

        static JsonHelper()
        {
            IsoDateTimeConverter datetimeConverter = new IsoDateTimeConverter();
            datetimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            _jsonSettings = new JsonSerializerSettings();
            _jsonSettings.MissingMemberHandling = MissingMemberHandling.Error;
            _jsonSettings.NullValueHandling = NullValueHandling.Ignore;
            _jsonSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            _jsonSettings.Converters.Add(datetimeConverter);
            _jsonSettings.Converters.Add(new IpAddressConverter());
            _jsonSettings.Converters.Add(new IpEndPointConverter());
        }


        //格式化json字符串
        public static string ConvertJsonString(string str)
        {
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object? obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return str;
            }
        }


        /// <summary>
        /// 将指定的对象序列化成 JSON 数据。
        /// </summary>
        /// <param name="obj">要序列化的对象。</param>
        /// <param name="formatting"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string? ToJson(this object obj, Formatting formatting = Formatting.None, MissingMemberHandling p = MissingMemberHandling.Error)
        {
            if (obj == null)
            {
                return null;
            }

            _jsonSettings.MissingMemberHandling = p;
            try
            {
                return JsonConvert.SerializeObject(obj, formatting, _jsonSettings);
            }
            catch (Exception ex)
            {
                Log.Logger.Error($"[{_loggerHead}]->Json序列化异常 => \r\n {ex.Message} \r\n {ex.StackTrace}");
                return null!;
            }
        }

        /// <summary>
        /// 将指定的 JSON 数据反序列化成指定对象。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="json">JSON 数据。</param>
        /// <param name="p"></param>
        /// <returns></returns>
        /// MissingMemberHandling.Ignore实体类缺少字段时忽略它
        public static T FromJson<T>(this string json, MissingMemberHandling p = MissingMemberHandling.Ignore)
        {
            _jsonSettings.MissingMemberHandling = p;

            try
            {
                return JsonConvert.DeserializeObject<T>(json, _jsonSettings)!;
            }
            catch (Exception ex)
            {
                Log.Logger.Error($"[{_loggerHead}]->Json返序列化异常=> \r\n ===== {ex.Message} \r\n ===== {ex.StackTrace} \r\n ===== json内容：{json} \r\n ===== 转换格式:{typeof(T)}");
                return default(T)!;
            }
        }
    }
}
