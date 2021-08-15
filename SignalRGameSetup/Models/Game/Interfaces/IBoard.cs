using SignalRGameSetup.Models.Game.Board;

namespace SignalRGameSetup.Models.Game.Interfaces
{
    interface IBoard
    {
        string GameCode { get; set; }
        EnemyBoardHalf EnemyBoard { get; set; }
        PlayerBoardHalf PlayerBoard { get; set; }
    }
}
