using SignalRGameSetup.Models.Game.Board.Pieces.Ship;
using SignalRGameSetup.Models.Game.Interfaces;

namespace SignalRGameSetup.Models.Game.Board.Pieces
{
    public class ShipPiece : IShipPiece
    {
        public string Name { get; set; }
        public int Length { get; set; }
        public string Direction { get; set; } // TODO consider using an enum
        public ShipPosition[] Positions { get; set; }
        public bool IsSet { get; set; }
        public bool IsSunk { get; set; }

        public ShipPiece(string name, int length)
        {
            Name = name;
            Length = length;
            Direction = null;
            Positions = new ShipPosition[length];
            IsSet = false;
            IsSunk = false;
        }

    }
}