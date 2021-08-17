namespace SignalRGameSetup.Database.Dtos
{
    public class BattleShipDto
    {
        public string GameCode { get; set; }
        public string ActivePlayerId { get; set; } // same ID as setup
        public string PlayerOne { get; set; } // JSON string
        public int PlayerOneBoard { get; set; } // A database ID
        public string PlayerTwo { get; set; } // JSON 
        public int PlayerTwoBoard { get; set; } // A database ID
        public int BattleShipInfo { get; set; } // A database ID
    }
}