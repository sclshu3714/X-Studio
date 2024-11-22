using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Common.Extensions {
    public static class JsonConvertExtension {
        public static string ToJson(this object obj) {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static T? ToObject<T>(this string json) {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
    }
}
