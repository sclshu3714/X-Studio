using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace XStudio.Converters
{
    public class BoolConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(bool) || objectType == typeof(Boolean));
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            bool b = (bool)value!;
            if (b)
            {
                writer.WriteValue("1");
            }
            else
            {
                writer.WriteValue("0");
            }
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue,
            JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            string? tokenValue = token.Value<string>();
            if (tokenValue != null && tokenValue.Trim().Equals("1"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
