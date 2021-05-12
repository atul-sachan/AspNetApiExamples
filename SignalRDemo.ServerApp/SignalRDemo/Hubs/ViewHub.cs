using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRDemo.Hubs
{
    public class ViewHub : Hub
    {
        public static int ViewCount { get; set; } = 0;
        public ViewHub()
        { }

        public async Task NotifyWatching()
        {
            ViewCount++;
            await this.Clients.All.SendAsync("viewCountUpdate", ViewCount);
        }

        public Task IncrementServerView()
        {
            ViewCount++;
            return Clients.All.SendAsync("incrementView", ViewCount);
        }

    }
}
