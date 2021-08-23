using Microsoft.AspNet.SignalR;
using SignalRGameSetup.Helpers.Game;
using SignalRGameSetup.Helpers.Setup;
using SignalRGameSetup.Models.Game;
using SignalRGameSetup.Models.Game.Containers;
using SignalRGameSetup.Models.Setup;
using SignalRGameSetup.Models.Setup.Interfaces;
using System;

namespace SignalRGameSetup.Hubs
{
    public class GameHub : Hub
    {
        public void ConnectGame(GameSetup setup)
        {
            // connect user to group
            Groups.Add(Context.ConnectionId, setup.GameCode);

            // Get the player who is reaching this method
            IParticipant player = SetupHelper.GetParticipantByConnectionId(Context.ConnectionId);

            // throw an error if null
            if (player == null)
            {
                throw new Exception("Error: no player has that connection id.");
            }

            // attempt to grab a game
            BattleShipsGame game = GameHelper.GetGameFromInfo(setup.GameCode, player.ParticipantId);

            // if it doesn't exist, create one
            if (game == null)
            {
                // get the enemy too
                IParticipant enemy = null;
                for (int i = 0; i < 2; i++)
                {
                    if (player.ParticipantId != setup.Players[i].ParticipantId)
                    {
                        enemy = setup.Players[i];
                        break;
                    }
                }

                game = new BattleShipsGame(setup.GameCode);
                // set player one to this player - don't worry about checking if
                // there's an active player since the game is null and we are creating a new game
                game.Information.ActivePlayerId = player.ParticipantId;
                game.Board.PlayerBoard.GameCode = setup.GameCode;
                // set enemy to other player
                game.Board.EnemyBoard.GameCode = setup.GameCode;
                if (enemy != null)
                {
                    game.Board.PlayerBoard.ParticipantId = player.ParticipantId;
                    game.Board.EnemyBoard.ParticipantId = enemy.ParticipantId;
                }

                // add the game to the database
                GameHelper.AddGame(game);

                //// now grab the game again since it has the BoardIds
                //game = null;
                //game = GameHelper.GetGameFromInfo(setup.GameCode, player.ParticipantId);

            }
            // if the game DOES exist,
            else
            {
                // It SHOULD have set everything up correctly already
            }

            // now update the game!!
            Clients.Client(Context.ConnectionId).setGame(game);

            // TODO decide what to do if someone is just watching
        }


        // Create fake game
        //model.Game = new BattleShipsGame("TEST");
        //model.Game.Information.ActivePlayerId = "TST1";
        //    model.Game.Board.PlayerBoard.ParticipantId = "TST1";
        //    model.Game.Board.EnemyBoard.ParticipantId = "TST2";

        public void GetTestGame()
        {
            // create fake setup
            TestModel model = new TestModel();
            GameSetup setup = new GameSetup();
            setup.GameCode = "TEST";
            model.TestSetup = setup;
            Player player1 = new Player("Player 1", "000", setup.GameCode);
            player1.ParticipantId = "TST1";

            Player player2 = new Player("Player 2", "000", setup.GameCode);
            player2.ParticipantId = "TST2";

            setup.AllowAudience = false;
            model.ParticipantId = "TST1";

            string activeParticipantId;

            // determine whether to erase a game and start a new one
            bool startNewGame = false;

            // try to grab the setup from the repo - if it comes back null, create it.
            GameSetup existingSetup = SetupHelper.GetSetupByGameCode(setup.GameCode);
            if (existingSetup == null)
            {
                player1.ConnectionId = Context.ConnectionId;
                activeParticipantId = player1.ParticipantId;
                setup.Players.Add(player1);
                setup.Players.Add(player2);

                SetupHelper.AddGameSetup(setup);
                startNewGame = true;
            }
            else
            {
                // HACK Opening more than two tabs will reset the game - for the testing phase
                if (existingSetup.Players.Count == 2)
                {

                    // If one of the players has a connectionId = 000, set it to that player
                    if (existingSetup.Players[0].ConnectionId == "000")
                    {
                        existingSetup.Players[0].ConnectionId = Context.ConnectionId;
                        activeParticipantId = existingSetup.Players[0].ParticipantId;
                        SetupHelper.UpdateGameSetup(existingSetup);
                        setup = existingSetup;
                        startNewGame = true;
                    }
                    else if (existingSetup.Players[1].ConnectionId == "000")
                    {
                        existingSetup.Players[1].ConnectionId = Context.ConnectionId;
                        activeParticipantId = existingSetup.Players[1].ParticipantId;
                        SetupHelper.UpdateGameSetup(existingSetup);
                        setup = existingSetup;
                    }
                    else // if neither of them have 000s, update the database game with new setup
                    {
                        player1.ConnectionId = Context.ConnectionId;
                        activeParticipantId = player1.ParticipantId;
                        setup.Players.Add(player1);
                        setup.Players.Add(player2);

                        SetupHelper.UpdateGameSetup(setup);
                        startNewGame = true;
                    }
                }
                else // if it has the wrong number of players, start a new setup. This is a test so it's ok.
                {
                    player1.ConnectionId = Context.ConnectionId;
                    activeParticipantId = player1.ParticipantId;
                    setup.Players.Add(player1);
                    setup.Players.Add(player2);

                    SetupHelper.UpdateGameSetup(setup);
                    startNewGame = true;
                }

            }

            // if starting a new game, delete the old one in the database
            if (startNewGame)
            {
                BattleShipsGame dbGame = GameHelper.GetGameFromInfo(setup.GameCode, activeParticipantId);

                // if it's not null, delete the game
                if (dbGame != null)
                {
                    GameHelper.DeleteGame(setup.GameCode);
                }

            }

            model.ParticipantId = activeParticipantId;
            model.TestSetup = setup;

            Clients.Caller.setTestGame(model);

        }

        public void GetGameInformation(GetGameInformation info)
        {
            BattleShipsGame battleShips = new BattleShipsGame(info.Setup.GameCode);

            // attempt to grab it from the database

            // If unsuccessful, create a new board
            // HACK for now it is hard coded to create this


            // store all the information according to who the player is - check
            // the participantIds in the game setup inside info... Set player 1 to the one that matches
            // and player 2 to the other
        }

    }
}