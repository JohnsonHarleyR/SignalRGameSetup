using Microsoft.AspNet.SignalR;
using SignalRGameSetup.Helpers.Setup;
using SignalRGameSetup.Models.Setup;
using SignalRGameSetup.Models.Setup.Containers;
using SignalRGameSetup.Models.Setup.Interfaces;
using System.Threading.Tasks;

namespace SignalRGameSetup.Hubs
{
    public class SetupHub : Hub
    {
        //public void UpdateOnDisconnect(string gameCode, string participantId)
        //{
        //    // if the values are not null, remove user from appropriate setup list
        //    if (gameCode != null && participantId != null)
        //    {
        //        // get the setup based on the game code
        //        GameSetup setup = SetupHelper.GetSetupByGameCode(gameCode);

        //        // remove the participant
        //        IParticipant participant = null;

        //        // first look through the players
        //        foreach (var player in setup.Players)
        //        {
        //            if (player.ParticipantId == participantId)
        //            {
        //                // if it matches, remove that player
        //                participant = player;
        //                setup.Players.Remove(player);
        //                break;
        //            }
        //        }

        //        // if participant is still null, check the watchers
        //        if (participant == null)
        //        {
        //            foreach (var watcher in setup.Watchers)
        //            {
        //                if (watcher.ParticipantId == participantId)
        //                {
        //                    // if it matches, remove that watcher
        //                    participant = watcher;
        //                    setup.Watchers.Remove(watcher);
        //                    break;
        //                }
        //            }
        //        }

        //        // now save the setup
        //        SetupHelper.UpdateGameSetup(setup);

        //        // if participant isn't null, update the wait room
        //        if (participant != null)
        //        {
        //            Clients.Group(gameCode).updateGameSetup(setup);
        //        }

        //    }
        //}

        //public override Task OnDisconnected(bool stopCalled)
        //{
        //    if (stopCalled)
        //    {
        //        //We know that Stop() was called on the client,
        //        //and the connection shut down gracefully.

        //        // get the game code and participant's id from cookies - if they exist
        //        //var httpContext = Context.Request.GetHttpContext();
        //        //UserConnection user = UserHandler.UserList.Find(x => x.ConnectionId == Context.ConnectionId);

        //        Cookie cookie;
        //        string gameCode = Context.RequestCookies.TryGetValue("GameCode", out cookie)
        //            ? cookie.Value : null;
        //        string participantId = Context.RequestCookies.TryGetValue("ParticipantId", out cookie)
        //            ? cookie.Value : null;

        //        //if (httpContext.Request.Cookies["GameCode"] != null)
        //        //{
        //        //    gameCode = httpContext.Request.Cookies["GameCode"].Value;
        //        //}
        //        //if (httpContext.Request.Cookies["ParticipantId"] != null)
        //        //{
        //        //    participantId = httpContext.Request.Cookies["ParticipantId"].Value;
        //        //}

        //        // if the values are not null, remove user from appropriate setup list
        //        if (gameCode != null && participantId != null)
        //        {
        //            // get the setup based on the game code
        //            GameSetup setup = SetupHelper.GetSetupByGameCode(gameCode);

        //            // remove the participant
        //            IParticipant participant = null;

        //            // first look through the players
        //            foreach (var player in setup.Players)
        //            {
        //                if (player.ParticipantId == participantId)
        //                {
        //                    // if it matches, remove that player
        //                    participant = player;
        //                    setup.Players.Remove(player);
        //                    break;
        //                }
        //            }

        //            // if participant is still null, check the watchers
        //            if (participant == null)
        //            {

        //                // remove the from the group

        //                foreach (var watcher in setup.Watchers)
        //                {
        //                    if (watcher.ParticipantId == participantId)
        //                    {
        //                        // if it matches, remove that watcher
        //                        participant = watcher;
        //                        setup.Watchers.Remove(watcher);
        //                        break;
        //                    }
        //                }
        //            }

        //            // now save the setup
        //            SetupHelper.UpdateGameSetup(setup);

        //            // if participant isn't null, update the wait room
        //            if (participant != null)
        //            {
        //                Clients.Group(gameCode).updateGameSetup(setup);
        //            }

        //        }

        //    }
        //    else
        //    {
        //        // This server hasn't heard from the client in the last ~35 seconds.
        //        // If SignalR is behind a load balancer with scaleout configured, 
        //        // the client may still be connected to another SignalR server.
        //    }

        //    return base.OnDisconnected(stopCalled);
        //}

        public override Task OnDisconnected(bool stopCalled)
        {
            if (stopCalled)
            {
                //We know that Stop() was called on the client,
                //and the connection shut down gracefully.

                // get the game code and participant's id
                //var httpContext = Context.Request.GetHttpContext();
                //UserConnection user = UserHandler.UserList.Find(x => x.ConnectionId == Context.ConnectionId);

                IParticipant participant = SetupHelper.GetParticipantByConnectionId(Context.ConnectionId);
                string gameCode = null;
                string participantId = null;
                if (participant != null)
                {
                    gameCode = participant.GameCode;
                    participantId = participant.ParticipantId;
                }

                IParticipant foundParticipant = null;

                // if the values are not null, remove user from appropriate setup list
                if (gameCode != null && participantId != null)
                {
                    // get the setup based on the game code
                    GameSetup setup = SetupHelper.GetSetupByGameCode(gameCode);

                    // first look through the players
                    foreach (var player in setup.Players)
                    {
                        if (player.ParticipantId == participantId)
                        {
                            // if it matches, remove that player
                            foundParticipant = player;
                            setup.Players.Remove(player);
                            break;
                        }
                    }

                    // if participant is still null, check the watchers
                    if (foundParticipant == null)
                    {

                        // remove the from the group

                        foreach (var watcher in setup.Watchers)
                        {
                            if (watcher.ParticipantId == participantId)
                            {
                                // if it matches, remove that watcher
                                foundParticipant = watcher;
                                setup.Watchers.Remove(watcher);
                                break;
                            }
                        }
                    }

                    // now save the setup
                    SetupHelper.UpdateGameSetup(setup);

                    // if participant isn't null, update the wait room
                    if (foundParticipant != null)
                    {
                        Clients.Group(gameCode).updateGameSetup(setup);
                    }

                }

            }
            else
            {
                // This server hasn't heard from the client in the last ~35 seconds.
                // If SignalR is behind a load balancer with scaleout configured, 
                // the client may still be connected to another SignalR server.
            }

            return base.OnDisconnected(stopCalled);
        }

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
            Clients.Client(Context.ConnectionId).setClientId(participant.ParticipantId);
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
            Clients.Client(Context.ConnectionId).setClientId(participant.ParticipantId);
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