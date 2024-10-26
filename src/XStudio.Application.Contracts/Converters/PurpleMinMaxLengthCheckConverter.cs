using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Converters {
    internal class PurpleMinMaxLengthCheckConverter : JsonConverter {
        public override bool CanConvert(Type t) => t == typeof(string);

        public override object ReadJson(JsonReader reader, Type t, object? existingValue, JsonSerializer serializer) {
            var value = serializer.Deserialize<string>(reader);
            if (value != null && value.Length >= 0 && value.Length <= 32) {
                return value;
            }
            throw new Exception("Cannot unmarshal type string");
        }

        public override void WriteJson(JsonWriter writer, object? untypedValue, JsonSerializer serializer) {
            var value = untypedValue == null ? "" : (string)untypedValue;
            if (value.Length >= 0 && value.Length <= 32) {
                serializer.Serialize(writer, value);
                return;
            }
            throw new Exception("Cannot marshal type string");
        }

        public static readonly PurpleMinMaxLengthCheckConverter Singleton = new PurpleMinMaxLengthCheckConverter();
    }

    internal class FluffyMinMaxLengthCheckConverter : JsonConverter {
        public override bool CanConvert(Type t) => t == typeof(string);

        public override object ReadJson(JsonReader reader, Type t, object? existingValue, JsonSerializer serializer) {
            var value = serializer.Deserialize<string>(reader);
            if (value != null && value.Length >= 0 && value.Length <= 255) {
                return value;
            }
            throw new Exception("Cannot unmarshal type string");
        }

        public override void WriteJson(JsonWriter writer, object? untypedValue, JsonSerializer serializer) {
            var value = untypedValue == null ? "" : (string)untypedValue;
            if (value.Length >= 0 && value.Length <= 255) {
                serializer.Serialize(writer, value);
                return;
            }
            throw new Exception("Cannot marshal type string");
        }

        public static readonly FluffyMinMaxLengthCheckConverter Singleton = new FluffyMinMaxLengthCheckConverter();
    }
}
