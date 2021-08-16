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
        public bool IsEnteringGameSetup { get; set; }
        public bool IsEnteringGameChat { get; set; }

        public Watcher()
        {
            ParticipantId = SetupHelper.GenerateParticipantId();
        }

        public Watcher(string name, string connectionId, string gameCode)
        {
            Name = name;
            ParticipantId = SetupHelper.GenerateParticipantId();
            ConnectionId = connectionId;
            GameCode = gameCode;
        }

    }
}