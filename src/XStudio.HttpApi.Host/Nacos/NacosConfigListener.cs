using Microsoft.Extensions.Hosting;
using Nacos.V2.Utils;
using Nacos.V2;

namespace XStudio.Nacos
{
    public class NacosConfigListener : IListener
    {
        private readonly IHostApplicationLifetime _appLifetime;
        public NacosConfigListener(IHostApplicationLifetime appLifetime)
        {
            _appLifetime = appLifetime;
        }
        public void ReceiveConfigInfo(string configInfo)
        {
            // 这里会有配置变更的回调，在这里处理配置变更之后的逻辑。
            System.Console.WriteLine("监听到配置修改了 => " + configInfo);
            //GlobalSettings.Instance.WebConfig = configInfo.ToObj<AKStreamWebConfig>();
            //_appLifetime.StopApplication();
        }
    }
}
