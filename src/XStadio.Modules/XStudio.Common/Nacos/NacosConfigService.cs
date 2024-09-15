using Microsoft.Extensions.DependencyInjection;
using Nacos.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Volo.Abp.DependencyInjection;

namespace XStudio.Common.Nacos
{
    public static class NacosConfigService
    {
        private static INacosConfigService? nacosConfigService = null;
        private static List<Action<string>> actions = new List<Action<string>>();
        private static string? currentConfig = null;

        /// <summary>
        /// 开启监听nacos应用配置
        /// </summary>
        /// <param name="app"></param>
        /// <param name="dataId">配置ID</param>
        public static async void AddNacosApplicationpCongfig(this IServiceCollection services, string dataId, string group = "application", string? Format = "JSON", long timeoutMs = 3000)
        {
            // 从服务容器中获取 INacosConfigService
            nacosConfigService = services.GetRequiredService<INacosConfigService>();
            currentConfig = await nacosConfigService.GetConfig(dataId, group, timeoutMs);
        }
    }
}
