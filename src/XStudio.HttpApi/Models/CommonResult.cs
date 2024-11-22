using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Models {
    /// <summary>
    /// 新基础请求结果返回类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommonResult<T> {
        private HttpStatusCode _httpStatusCode = HttpStatusCode.OK;
        public string Code { get; set; } = "0";
        public string Message { get; set; } = "Success";
        public T? Data { get; set; } = default;

        public HttpStatusCode HttpStatusCode {
            get => _httpStatusCode;
            set {
                if (value < 0) {
                    throw new ArgumentOutOfRangeException(nameof(value), "HTTP status code cannot be negative.");
                }
                _httpStatusCode = value;
            }
        }

        /// <summary>
        /// 为JSON字符串转换将CommonResult
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T? Parse(string json) {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 将Data转换为指定类型,最多支持两层，1直接将Data转换为指定类型，2如果Data是JObject，则尝试从指定keyfield中获取值并转换为指定类型
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="keyfield"></param>
        /// <returns></returns>
        public U? DataAs<U>(string keyfield = "data") {
            if (Data is JObject dataObject) {
                if (dataObject.ContainsKey(keyfield) && dataObject[keyfield] is JObject fieldObject) {
                    return fieldObject.ToObject<U>();
                }
                else {
                    return dataObject.ToObject<U>();
                }
            }
            return default;
        }
    }
}
