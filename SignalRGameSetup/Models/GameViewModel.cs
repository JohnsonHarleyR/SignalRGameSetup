using SignalRGameSetup.Enums.Setup.ActionEnums;
using SignalRGameSetup.Models.Setup;

namespace SignalRGameSetup.Models
{
    public class GameViewModel
    {
        //public IParticipant Participant { get; set; }
        public string Name { get; set; }
        public string ConnectionId { get; set; }
        public ParticipantType ParticipantType { get; set; }
        public ActionType ActionType { get; set; }
        public GameSetup Setup { get; set; }
        public string Message { get; set; }
    }
}