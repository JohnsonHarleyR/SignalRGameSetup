using Microsoft.AspNet.SignalR;
using Owin;

namespace SignalRGameSetup
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR("/signalr", new HubConfiguration());
        }
    }
}
