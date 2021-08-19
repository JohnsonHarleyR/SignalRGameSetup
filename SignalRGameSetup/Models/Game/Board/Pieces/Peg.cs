using SignalRGameSetup.Enums.Game;

namespace SignalRGameSetup.Models.Game.Board.Pieces
{
    public class Peg
    {
        public int XPos { get; set; }
        public YPosition YPos { get; set; }
        private PegColor? _color = null;
        public PegColor? Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                switch (value)
                {
                    default:
                    case null:
                        _className = null;
                        break;
                    case PegColor.White:
                        _className = "peg miss";
                        break;
                    case PegColor.Red:
                        _className = "peg hit";
                        break;
                }
            }
        }
        private string _className = null;
        public string ClassName
        {
            get;
        }

    }
}