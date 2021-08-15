using SignalRGameSetup.Models.Game.Board.Pieces.Ship;
using SignalRGameSetup.Models.Game.Interfaces;

namespace SignalRGameSetup.Models.Game.Board.Pieces
{
    public class ShipPiece : IShipPiece
    {
        public string Name { get; set; }
        public int Length { get; set; }
        public ShipPosition[] Positions { get; set; }
        public bool IsSunk { get; set; }

        public ShipPiece(string name, int length)
        {
            Name = name;
            Length = length;
            Positions = new ShipPosition[length];
            IsSunk = false;
        }

    }
}