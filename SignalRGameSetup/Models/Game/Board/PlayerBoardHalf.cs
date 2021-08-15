using SignalRGameSetup.Models.Game.Interfaces;

namespace SignalRGameSetup.Models.Game.Board
{
    public class PlayerBoardHalf : IBoardHalf
    {
        public string ParticipantId { get; set; }
        public int BoardSize { get; set; }

    }
}