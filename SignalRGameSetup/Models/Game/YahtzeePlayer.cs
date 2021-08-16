namespace SignalRGameSetup.Models.Game
{
    public class YahtzeePlayer
    {
        // TODO allow the user to set an icon for their player to display in the game :)
        // TODO make it so two players cannot have the same name
        public string Name { get; set; }
        public string ParticipantId { get; set; }
        public string GameCode { get; set; }
        public ScoreSheet Scoresheet { get; set; } = new ScoreSheet();
        public ScoreSheet TheoreticalScores { get; set; }
        public int RollsLeft { get; set; } = 3;
        public string ScoreToChange { get; set; } = null;
        public string DiceColor { get; set; } = "default";
    }
}