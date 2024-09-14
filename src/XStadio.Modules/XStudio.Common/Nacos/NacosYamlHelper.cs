using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using Nacos.V2;
using YamlDotNet.Serialization.NamingConventions;

namespace XStudio.Common.Nacos
{
    public class NacosYamlHelper
    {
        private readonly INacosConfigService _nacosConfigService;
        private readonly IDeserializer _deserializer;
        private readonly ISerializer _serializer;

        public NacosYamlHelper(INacosConfigService nacosConfigService)
        {
            _nacosConfigService = nacosConfigService;
            _deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            _serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
        }

        public async Task<Dictionary<string, object>> GetYamlConfigAsync(string dataId, string group)
        {
            var yamlContent = await _nacosConfigService.GetConfig(dataId, group, 3000);
            if (string.IsNullOrEmpty(yamlContent))
            {
                throw new Exception("Failed to retrieve YAML content from Nacos.");
            }

            var yamlObject = _deserializer.Deserialize<Dictionary<string, object>>(yamlContent);
            return yamlObject;
        }

        public async Task<T> GetYamlConfigAsync<T>(string dataId, string group)
        {
            var yamlContent = await _nacosConfigService.GetConfig(dataId, group, 3000);
            if (string.IsNullOrEmpty(yamlContent))
            {
                throw new Exception("Failed to retrieve YAML content from Nacos.");
            }

            var yamlObject = _deserializer.Deserialize<T>(yamlContent);
            return yamlObject;
        }



        public async Task<bool> PublishYamlConfigAsync(string dataId, string group, Dictionary<string, object> yamlObject)
        {
            var yamlContent = _serializer.Serialize(yamlObject);
            return await _nacosConfigService.PublishConfig(dataId, group, yamlContent, "yaml");
        }

        public async Task<bool> RemoveYamlConfigAsync(string dataId, string group)
        {
            return await _nacosConfigService.RemoveConfig(dataId, group);
        }
    }
}
