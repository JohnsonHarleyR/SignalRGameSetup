using SignalRGameSetup.Models.Game.Interfaces;

namespace SignalRGameSetup.Models.Game.Board
{
    public class GuessBoardHalf : IBoardHalf
    {
        public string ParticipantId { get; set; }

        public GuessBoardHalf() { }
        public GuessBoardHalf(string participantId)
        {
            ParticipantId = participantId;
        }

    }
}