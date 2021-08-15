using SignalRGameSetup.Enums.Game;

namespace SignalRGameSetup.Models.Game.Board.Pieces.Ship
{
    public class ShipPosition
    {
        public int XPos { get; set; }
        public YPosition YPos { get; set; }
        public string Image { get; set; }
        public bool IsHit { get; set; }

    }
}