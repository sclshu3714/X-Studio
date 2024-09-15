using Microsoft.Extensions.Hosting;
using Nacos.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Common.Nacos
{
    public class NacosConfigListener : IListener, IHostedService
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

        private void OnStarted() => Console.WriteLine("Application Started");
        private void OnStopping() => Console.WriteLine("Application Stopping");
        private void OnStopped() => Console.WriteLine("Application Stopped");

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(() => OnStarted());
            _appLifetime.ApplicationStopping.Register(() => OnStopping());
            _appLifetime.ApplicationStopped.Register(() => OnStopped());
            return Task.CompletedTask;
        }
    }
}
