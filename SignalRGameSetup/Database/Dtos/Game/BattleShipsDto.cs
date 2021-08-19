namespace SignalRGameSetup.Database.Dtos
{
    public class BattleShipsDto
    {
        public string GameCode { get; set; }
        public string PlayerOne { get; set; } // JSON string
        public int PlayerOneBoard { get; set; } // A database ID
        public string PlayerTwo { get; set; } // JSON 
        public int PlayerTwoBoard { get; set; } // A database ID
        public string Information { get; set; } // A JSON string with BattleShipInfo
    }
}