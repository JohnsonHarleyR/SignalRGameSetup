namespace SignalRGameSetup.Database.Dtos.Game
{
    public class YahtzeePlayerDto
    {
        public string Name { get; set; }
        public string ParticipantId { get; set; }
        public string GameCode { get; set; }
        public int ScoresheetId { get; set; }
        public int RollsLeft { get; set; }
        public string DiceColor { get; set; }
    }
}