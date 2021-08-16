namespace SignalRGameSetup.Database.Dtos.Game
{
    public class DieDto
    {
        public int TableId { get; set; }
        public int GameId { get; set; }
        public int DieCountId { get; set; } // this will be die 0-4
        public int Value { get; set; }
    }
}