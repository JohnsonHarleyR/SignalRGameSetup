using SignalRGameSetup.Models.Game.Interfaces;

namespace SignalRGameSetup.Models.Game.Board
{
    public class FullBoard : IBoard
    {
        public string GameCode { get; set; }
        public EnemyBoardHalf EnemyBoard { get; set; }
        public PlayerBoardHalf PlayerBoard { get; set; }

    }

}