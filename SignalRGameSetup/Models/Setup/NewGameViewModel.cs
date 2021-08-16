using SignalRGameSetup.Models.Chat.Containers;

namespace SignalRGameSetup.Models.Setup
{
    public class NewGameViewModel
    {
        public string GameCode { get; set; }
        public string ParticipantId { get; set; }
        public GameChat Chat { get; set; }
    }
}