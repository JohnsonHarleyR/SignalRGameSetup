using SignalRGameSetup.Models.Setup;

namespace SignalRGameSetup.Models.Game.Containers
{
    public class GetGameInformation
    {
        public GameSetup Setup { get; set; }
        public string ParticipantId { get; set; }
    }
}