using SignalRGameSetup.Models.Game.Board.Pieces;
using System.Collections.Generic;

namespace SignalRGameSetup.Models.Game.Interfaces
{
    interface IBoardHalf
    {
        int? BoardId { get; set; }
        string ParticipantId { get; set; }
        string GameCode { get; set; }
        Dictionary<string, BoardPosition> Positions { get; set; }

    }
}
