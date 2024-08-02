﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;

namespace XStudio.Converters
{
    public class IpAddressConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IPAddress));
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            IPAddress ip = (IPAddress)value!;
            writer.WriteValue(ip.ToString());
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue,
            JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            string? ipString = token.Value<string>();
            return string.IsNullOrWhiteSpace(ipString) ? null : IPAddress.Parse(ipString);
        }
    }
}
