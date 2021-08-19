using Newtonsoft.Json;
using SignalRGameSetup.Database.Dtos;
using SignalRGameSetup.Models.Game.Board;

namespace SignalRGameSetup.Models.Game
{
    public class BattleShipsGame
    {
        public string GameCode { get; set; }
        public FullBoard Board { get; set; }
        public BattleShipsInfo Information { get; set; } // A JSON string

        public BattleShipsGame() { }
        public BattleShipsGame(string gameCode)
        {
            GameCode = gameCode;
            Board = new FullBoard(gameCode);
            Information = new BattleShipsInfo();
        }

        public BattleShipsGame(BattleShipsGameDto dto)
        {
            GameCode = dto.GameCode;
            Board = new FullBoard(dto.GameCode);
            Information = (BattleShipsInfo)JsonConvert.DeserializeObject(dto.Information);


        }
    }
}