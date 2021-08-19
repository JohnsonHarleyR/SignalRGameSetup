using SignalRGameSetup.Enums.Game;

namespace SignalRGameSetup.Models.Game.Board.Pieces.Ship
{
    public class ShipPosition
    {
        public string Name
        {
            get
            {
                string name = $"{(int)YPos}-{XPos}";
                return name;
            }
        }
        public int XPos { get; set; }
        public YPosition YPos { get; set; }
        public string Image { get; set; }
        public bool IsHit { get; set; }
        //public bool HasPeg
        //{
        //    get
        //    {
        //        if (Peg == null)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //}
        //public Peg Peg { get; set; }

        public ShipPosition()
        {
            Image = null;
            IsHit = false;
            //Peg = null;
        }

        public ShipPosition(YPosition yPos, int xPos)
        {
            YPos = yPos;
            XPos = xPos;
            Image = null;
            IsHit = false;
            //Peg = null;
        }

    }
}