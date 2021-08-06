using Microsoft.AspNet.SignalR;
using SignalRGameSetup.Helpers.Setup;
using SignalRGameSetup.Models.Setup;
using SignalRGameSetup.Models.Setup.Containers;

namespace SignalRGameSetup.Hubs
{
    public class SetupHub : Hub
    {
        public void NewRoom(NewRoom roomInfo)
        {
            // Generate a new game to set up
            GameSetup newSetup = new GameSetup(roomInfo.AllowAudience);
            Player firstPlayer = new Player(roomInfo.Name, Context.ConnectionId, newSetup.GameCode);
            bool successful = newSetup.AddPlayer(firstPlayer);

            // if successful, add the setup to a database to reference later
            if (successful)
            {
                SetupHelper.AddGameSetup(newSetup);
                Groups.Add(Context.ConnectionId, newSetup.GameCode);
                Clients.Caller.enterNewRoom(newSetup);
            }

        }
    }
}