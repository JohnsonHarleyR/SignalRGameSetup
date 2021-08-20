using SignalRGameSetup.Enums.Game;

namespace SignalRGameSetup.Models.Game.Board.Pieces
{
    public class Peg
    {
        public int XPos { get; set; }
        public YPosition YPos { get; set; }
        public PegColor? Color { get; set; }
        public string ClassName
        {
            get
            {
                switch (Color)
                {
                    default:
                    case null:
                        return null;
                    case PegColor.White:
                        return "peg miss";
                    case PegColor.Red:
                        return "peg hit";
                }
            }
        }

    }
}