using SignalRGameSetup.Helpers.Setup;
using SignalRGameSetup.Models.Setup.Interfaces;

namespace SignalRGameSetup.Models.Setup
{
    public class Watcher : IParticipant
    {
        public string Name { get; set; }
        public string ParticipantId { get; set; }
        public string ConnectionId { get; set; }
        public string GameCode { get; set; }

        public Watcher()
        {
            ParticipantId = SetupHelper.GenerateParticipantId();
        }

    }
}