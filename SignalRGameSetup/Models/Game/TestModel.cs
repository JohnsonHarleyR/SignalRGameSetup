using SignalRGameSetup.Models.Setup;

namespace SignalRGameSetup.Models.Game
{
    public class TestModel
    {
        public string ParticipantId { get; set; }
        public BattleShipsGame Game { get; set; }
        public GameSetup TestSetup { get; set; }
    }
}