using SignalRGameSetup.Helpers.Game;
using SignalRGameSetup.Models.Game.Board.Pieces;
using SignalRGameSetup.Models.Game.Interfaces;
using System.Collections.Generic;

namespace SignalRGameSetup.Models.Game.Board
{
    public class PlayerBoardHalf : IBoardHalf
    {
        public string ParticipantId { get; set; }
        public Dictionary<string, BoardPosition> Positions { get; set; }
        public Dictionary<string, ShipPiece> Ships { get; set; }

        public PlayerBoardHalf(string participantId)
        {
            ParticipantId = participantId;

            Ships = BoardHelper.CreateShips();
        }

    }
}