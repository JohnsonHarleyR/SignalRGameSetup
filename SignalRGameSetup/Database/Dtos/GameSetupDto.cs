using Newtonsoft.Json;
using SignalRGameSetup.Models.Setup;

namespace SignalRGameSetup.Database.Dtos
{
    public class GameSetupDto
    {
        public string GameCode { get; set; } // Code that gets generated in order to join a game - used as primary key in DB
        public string Players { get; set; } // A Json string
        public string Watchers { get; set; } // A Json string
        public bool AllowAudience { get; set; }
        public bool LeaveInDatabase { get; set; }

        public GameSetupDto() { }

        public GameSetupDto(GameSetup setup)
        {
            GameCode = setup.GameCode;
            Players = JsonConvert.SerializeObject(setup.Players);
            Watchers = JsonConvert.SerializeObject(setup.Watchers);
            AllowAudience = setup.AllowAudience;
            LeaveInDatabase = setup.LeaveInDatabase;
        }

    }
}