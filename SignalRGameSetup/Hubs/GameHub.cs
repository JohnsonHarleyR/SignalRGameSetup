using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRGameSetup.Hubs
{
    public class GameHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}