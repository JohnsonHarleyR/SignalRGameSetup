using SignalRGameSetup.Models.Game.Board.Pieces.Ship;

namespace SignalRGameSetup.Models.Game.Interfaces
{
    interface IShipPiece
    {
        string Name { get; set; }
        int Length { get; set; }
        ShipPosition[] Positions { get; set; }
        bool IsSunk { get; set; }
    }
}
