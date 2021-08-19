using SignalRGameSetup.Models.Game.Board.Pieces.Ship;
using SignalRGameSetup.Models.Game.Interfaces;
using System.Collections.Generic;

namespace SignalRGameSetup.Models.Game.Board.Pieces
{
    public class ShipPiece : IShipPiece
    {
        public string Name { get; set; }
        public int Length { get; set; }
        public string Direction { get; set; } // TODO consider using an enum
        public List<ShipPosition> Positions { get; set; }
        public bool IsSet { get; set; }
        public bool IsSunk { get; set; }

        public ShipPiece(string name, int length)
        {
            Name = name;
            Length = length;
            Direction = null;
            Positions = new List<ShipPosition>();
            IsSet = false;
            IsSunk = false;

            for (int i = 0; i < Positions.Count; i++)
            {
                Positions[i] = new ShipPosition();
            }
        }

    }
}