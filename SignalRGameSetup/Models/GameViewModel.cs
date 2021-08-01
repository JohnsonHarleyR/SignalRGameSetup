using SignalRGameSetup.Enums.Setup.ActionEnums;
using SignalRGameSetup.Models.Setup;
using SignalRGameSetup.Models.Setup.Interfaces;

namespace SignalRGameSetup.Models
{
    public class GameViewModel
    {
        public IParticipant Participant { get; set; }
        public ParticipantType ParticipantType { get; set; }
        public ActionType ActionType { get; set; }
        public GameSetup Setup { get; set; }
    }
}