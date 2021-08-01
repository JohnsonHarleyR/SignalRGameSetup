using SignalRGameSetup.Models.Setup.Interfaces;

namespace SignalRGameSetup.Models.Setup
{
    public class Watcher : IParticipant
    {
        public string Name { get; set; }
        public string ConnectionId { get; set; }
    }
}