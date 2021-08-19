using Microsoft.AspNet.SignalR;
using SignalRGameSetup.Models.Game;
using SignalRGameSetup.Models.Game.Containers;
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

        public void GetTestGame()
        {
            // create fake setup
            TestModel model = new TestModel();
            GameSetup setup = new GameSetup();
            setup.GameCode = "TEST";
            model.TestSetup = setup;
            Player player1 = new Player("Player 1", "12345", setup.GameCode);
            player1.ParticipantId = "TST1";
            setup.Players.Add(player1);
            Player player2 = new Player("Player 2", "67890", setup.GameCode);
            player1.ParticipantId = "TST2";
            setup.Players.Add(player2);
            setup.AllowAudience = false;
            model.ParticipantId = "TST1";

            // Create fake game
            model.Game = new BattleShipsGame("TEST");
            model.Game.Information.ActivePlayerId = "TST1";
            model.Game.Board.PlayerBoard.ParticipantId = "TST1";
            model.Game.Board.EnemyBoard.ParticipantId = "TST2";

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