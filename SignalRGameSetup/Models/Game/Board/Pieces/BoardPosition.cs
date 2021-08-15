using SignalRGameSetup.Enums.Game;
using SignalRGameSetup.Models.Game.Board.Pieces.Ship;

namespace SignalRGameSetup.Models.Game.Board.Pieces
{
    public class BoardPosition
    {
        public string Name
        {
            get
            {
                string name = $"{YPos}{XPos.ToString()}";
                return name;
            }
        }
        private int _xPos;
        public int XPos { get { return _xPos; } }
        private YPosition _yPos;
        public YPosition YPos { get { return _yPos; } }
        public bool HasShip
        {
            get
            {
                if (ShipPiece == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public ShipPosition ShipPiece { get; set; }
        public bool HasPeg
        {
            get
            {
                if (Peg == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public Peg Peg { get; set; }

        public BoardPosition(int xPos, YPosition yPos)
        {
            _xPos = xPos;
            _yPos = yPos;
        }

    }


}