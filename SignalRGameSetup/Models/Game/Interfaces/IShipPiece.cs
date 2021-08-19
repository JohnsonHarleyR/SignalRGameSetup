using SignalRGameSetup.Models.Game.Board.Pieces.Ship;
using System.Collections.Generic;

namespace SignalRGameSetup.Models.Game.Interfaces
{
    interface IShipPiece
    {
        string Name { get; set; }
        int Length { get; set; }
        string Direction { get; set; }
        List<ShipPosition> Positions { get; set; }
        bool IsSet { get; set; }
        bool IsSunk { get; set; }
    }
}
