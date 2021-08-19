using SignalRGameSetup.Enums.Game;
using SignalRGameSetup.Helpers.Game;
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
        public Dictionary<string, ShipPosition> Positions { get; set; }
        public bool IsSet { get; set; }
        public bool IsSunk { get; set; }
        public int HitsLeft
        {
            get
            {
                // loop through the positions and count how many are not sunk
                int count = 0;
                foreach (var pos in Positions)
                {
                    if (!pos.Value.IsHit)
                    {
                        count++;
                    }
                }
                // if there are no hits left, set is sunk to true
                if (count == 0)
                {
                    IsSunk = true;
                }
                return count;
            }
        }

        public ShipPiece(string name, int length)
        {
            Name = name;
            Length = length;
            Direction = null;
            Positions = new Dictionary<string, ShipPosition>();
            IsSet = false;
            IsSunk = false;

            for (int r = 1; r <= BoardHelper.GetBoardSize(); r++)
            {
                for (int c = 1; c <= BoardHelper.GetBoardSize(); c++)
                {
                    ShipPosition newPosition = new ShipPosition((YPosition)r, c);
                    Positions.Add(newPosition.Name, newPosition);
                }
            }
        }

    }
}