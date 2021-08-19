namespace SignalRGameSetup.Database.Dtos
{
    public class BattleShipsGameDto
    {
        public string GameCode { get; set; }
        public string PlayerOne { get; set; } // a participant id string
        public int PlayerOneBoard { get; set; } // A database ID
        public string PlayerTwo { get; set; } // a participant id string
        public int PlayerTwoBoard { get; set; } // A database ID
        public string Information { get; set; } // A JSON string with BattleShipInfo
    }
}