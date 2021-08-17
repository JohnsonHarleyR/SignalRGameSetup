using Microsoft.AspNet.SignalR;
using SignalRGameSetup.Models.Setup;

namespace SignalRGameSetup.Hubs
{
    public class GameHub : Hub
    {
        public void ConnectGame(GameSetup setup)
        {
            // connect user to group

            // attempt to grab a game - 

            // if it doesn't exist, create one
            // HACK for now this just creates one

            Clients.All.hello();
        }
    }
}