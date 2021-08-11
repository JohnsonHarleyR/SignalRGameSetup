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
            //newSetup.ActiveParticipant = firstPlayer;

            // if successful, add the setup to a database to reference later
            if (successful)
            {
                SetupHelper.AddGameSetup(newSetup);
                Groups.Add(Context.ConnectionId, newSetup.GameCode);
                // store the participant id on the view page
                Clients.Client(Context.ConnectionId).setClientId(firstPlayer.ParticipantId);
                Clients.Caller.enterRoom(newSetup);
            }

        }

        public void ExistingRoom(ExistingRoom fetchInfo)
        {
            // Generate a new game to set up
            GameSetup newSetup = SetupHelper.GetSetupByGameCode(fetchInfo.GameCode);

            // if setup is not null, add the setup to a database to reference later
            if (newSetup != null)
            {
                Groups.Add(Context.ConnectionId, newSetup.GameCode);
                Clients.Caller.decideHowToEnter(newSetup);
            }

        }

        public void JoinAsPlayer(JoinAsParticipant info)
        {
            GameSetup setup = info.Setup;

            // create a new participant and add the their information
            Player participant = new Player(info.Name, Context.ConnectionId, setup.GameCode);

            // add to list
            setup.Players.Add(participant);
            //setup.ActiveParticipant = participant;

            // update database
            SetupHelper.UpdateGameSetup(setup);

            // store the participant id on the view page
            //Clients.Client(Context.ConnectionId).setClientId(participant.ParticipantId);

            // return new game setup
            Clients.Group(info.Setup.GameCode, Context.ConnectionId).updateGameSetup(setup);
            Clients.Client(Context.ConnectionId).enterRoom(setup);
        }

        public void JoinAsWatcher(JoinAsParticipant info)
        {
            GameSetup setup = info.Setup;

            // create a new participant and add the their information
            Watcher participant = new Watcher(info.Name, Context.ConnectionId, setup.GameCode);

            // add to list
            setup.Watchers.Add(participant);
            //setup.ActiveParticipant = participant;

            // update database
            SetupHelper.UpdateGameSetup(setup);

            // store the participant id on the view page
            //Clients.Client(Context.ConnectionId).setClientId(participant.ParticipantId);

            // return new game setup
            Clients.Group(info.Setup.GameCode, Context.ConnectionId).updateGameSetup(setup);
            Clients.Client(Context.ConnectionId).enterRoom(setup);
        }

        public void IsValidCode(string code)
        {
            GameSetup newSetup = SetupHelper.GetSetupByGameCode(code);
            bool result = false;
            if (newSetup != null)
            {
                result = true;
            }
            Clients.Caller.setGameCodeBool(result);
        }

    }
}