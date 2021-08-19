namespace SignalRGameSetup.Database.Dtos.Game
{
    public class PlayerBoardHalfDto
    {
        public int BoardId { get; set; }
        public string ParticipantId { get; set; }
        public string GameCode { get; set; }
        public string Positions { get; set; }
        public string Ships { get; set; }
    }
}