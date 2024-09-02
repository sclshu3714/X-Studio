using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Hubs
{
    public class XStudioHubService
    {
        private readonly IHubContext<XStudioHub> _hubContext;

        public XStudioHubService(IHubContext<XStudioHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyClients(string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Server", message);
        }
    }
}
