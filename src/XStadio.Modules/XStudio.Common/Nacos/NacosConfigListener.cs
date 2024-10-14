using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RequestLocalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nacos.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Common.Nacos
{
    public static class NacosConfigListenerExtensions
    {
        public static IApplicationBuilder UseNacosConfigListener(this IApplicationBuilder app, Microsoft.Extensions.Configuration.IConfiguration configuration) {
            IHostApplicationLifetime appLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
            INacosConfigService ncsvc = app.ApplicationServices.GetRequiredService<INacosConfigService>();
            NacosConfigListener _configListen = new NacosConfigListener(appLifetime);
            // 遍历 Nacos:Listeners 内的值
            var listeners = configuration.GetSection("Nacos:Listeners").Get<List<NacosListener>>();
            if (listeners != null) {
                foreach (var listener in listeners) {
                    ncsvc.AddListener(listener.DataId, listener.Group, _configListen);
                }
            }
            return app;
        }
    }

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
