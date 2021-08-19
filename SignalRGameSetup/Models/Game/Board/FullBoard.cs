using SignalRGameSetup.Models.Game.Interfaces;

namespace SignalRGameSetup.Models.Game.Board
{
    public class FullBoard : IBoard
    {
        public string GameCode { get; set; }
        public GuessBoardHalf EnemyBoard { get; set; }
        public PlayerBoardHalf PlayerBoard { get; set; }

        public FullBoard(string gameCode)
        {
            GameCode = gameCode;
            EnemyBoard = new GuessBoardHalf();
            PlayerBoard = new PlayerBoardHalf();
        }

    }

}