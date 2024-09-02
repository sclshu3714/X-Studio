using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.App.Hubs
{
    public class XStudioSignalRClient
    {
        public XStudioSignalRClient() { }

        private HubConnection? _hubConnection;
        private IHubProxy? _hubProxy;

        public async void Connect(string connectionUrl, string hubName)
        {
            _hubConnection = new HubConnection(connectionUrl);
            _hubProxy = _hubConnection.CreateHubProxy(hubName);

            // 添加接收服务器发送的消息的回调方法
            _hubProxy.On<string>("ReceiveMessage", (message) =>
            {
                // 处理接收到的消息
                Console.WriteLine(message);
            });

            try
            {
                await _hubConnection.Start();
                Console.WriteLine("Connected to SignalR server");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void Send(string method, object[] args)
        {
            _hubProxy?.Invoke(method, args);
        }
    }
}
