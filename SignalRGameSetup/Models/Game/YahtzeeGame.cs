using Newtonsoft.Json;
using System.Collections.Generic;

namespace SignalRGameSetup.Models.Game
{
    public class YahtzeeGame
    {
        public string GameCode { get; set; }
        public List<Die> Dice { get; set; } // load this from the Yahtzee repo
        public List<YahtzeePlayer> Players { get; set; } = new List<YahtzeePlayer>(); // load from Yahtzee repo
        public YahtzeePlayer CurrentPlayer { get; set; } // get the current player from repo
        public string JsonString { get; set; }



        // get a JSON string of the model
        public string GetJSONString()
        {
            string jsonString = JsonConvert.SerializeObject(this);

            return jsonString;
        }
    }
}