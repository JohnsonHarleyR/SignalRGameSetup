using Microsoft.AspNet.SignalR;
using SignalRGameSetup.Helpers.Chat;
using SignalRGameSetup.Helpers.Setup;
using SignalRGameSetup.Models.Setup;
using SignalRGameSetup.Models.Setup.Containers;
using SignalRGameSetup.Models.Setup.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRGameSetup.Hubs
{
    public class SetupHub : Hub
    {
        // TODO Write tests!
        // TODO write more helper methods to minimize some of this code
        // HACK this method works around problems with browser cookies - consider changing back one uploaded online


        // Methods specific to the actual game page
        public void ConnectToGame(GoToGamePage info)
        {
            // get the correct setup
            GameSetup setup = SetupHelper.GetSetupByGameCode(info.GameCode);

            // update the player/watcher to have their new connection id in the database
            IParticipant participant = setup.Players.Where(p => p.ParticipantId == info.ParticipantId).FirstOrDefault();

            if (participant == null)
            {
                participant = setup.Watchers.Where(p => p.ParticipantId == info.ParticipantId).FirstOrDefault();
            }

            if (participant != null)
            {
                // TODO make sure this updates the setup correctly!!
                participant.ConnectionId = Context.ConnectionId;

                // indicate the participant is no longer entering the game
                participant.IsEnteringGameSetup = false;

            }

            //// change the setup to work properly if there's a disconnection
            //setup.LeaveInDatabase = false;


            // save the setup
            SetupHelper.UpdateGameSetup(setup);

            // update their client info on their page
            Clients.Client(Context.ConnectionId).setClientInfo(participant);

            // add this person to the group again
            Groups.Add(Context.ConnectionId, setup.GameCode);

            // update info on game page for everyone
            Clients.Group(setup.GameCode).updateGameSetup(setup);

            // load the chat for everyone
            Clients.Group(setup.GameCode).connectTheChat();
        }



        // Methods used/also used on the waiting page

        public void GoToGame(GoToGamePage info)
        {
            // get the setup and change the bool to tell it to leave in the database
            GameSetup setup = SetupHelper.GetSetupByGameCode(info.GameCode);

            // Try this... don't allow setup to be deleted, then redirect to game page
            //setup.LeaveInDatabase = true; // TODO set this back to false when directed to game page

            // New way - get all participants and set it so that they are entering a new game
            foreach (var player in setup.Players)
            {
                player.IsEnteringGameSetup = true;
                player.IsEnteringGameChat = true;
            }
            foreach (var watcher in setup.Watchers)
            {
                watcher.IsEnteringGameSetup = true;
                watcher.IsEnteringGameChat = true;
            }
            //IParticipant participant = SetupHelper.GetParticipantById(setup, info.ParticipantId);
            //participant.IsEnteringGame = true;

            SetupHelper.UpdateGameSetup(setup);

            // go back to setup page and have everyone direct to game page
            Clients.Group(info.GameCode).goToGamePage(setup);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (stopCalled)
            {
                //We know that Stop() was called on the client,
                //and the connection shut down gracefully.

                IParticipant participant = SetupHelper.GetParticipantByConnectionId(Context.ConnectionId);
                string gameCode = null;
                string participantId = null;
                if (participant != null)
                {
                    gameCode = participant.GameCode;
                    participantId = participant.ParticipantId;
                }

                IParticipant foundParticipant = null;

                // TODO put this next part inside a SetupHelper method
                // if the values are not null, remove user from appropriate setup list
                if (gameCode != null && participantId != null)
                {
                    // get the setup based on the game code
                    GameSetup setup = SetupHelper.GetSetupByGameCode(gameCode);

                    if (setup != null)
                    //if (setup != null && setup.LeaveInDatabase == false)
                    {
                        // first look through the players
                        foreach (var player in setup.Players)
                        {
                            if (player.ParticipantId == participantId)
                            {
                                // if it matches, remove that player
                                foundParticipant = player;
                                // if the participant isn't entering an new game, remove
                                if (!foundParticipant.IsEnteringGameSetup)
                                {
                                    setup.Players.Remove(player);
                                }
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
                                    // if the participant isn't entering an new game, remove
                                    if (!foundParticipant.IsEnteringGameSetup)
                                    {
                                        setup.Watchers.Remove(watcher);
                                    }
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

                        // if the setup has no players or watchers, delete it
                        // TODO decide if it should close when all players are gone and only watchers are left
                        if (setup.Players.Count == 0 && setup.Watchers.Count == 0)
                        {
                            SetupHelper.DeleteGameSetup(gameCode);
                            // Also delete associated chat
                            ChatHelper.DeleteGameChat(gameCode);
                        }
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