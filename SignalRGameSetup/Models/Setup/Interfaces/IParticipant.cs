namespace SignalRGameSetup.Models.Setup.Interfaces
{
    public interface IParticipant
    {
        string Name { get; set; }
        string ParticipantId { get; set; }
        string ConnectionId { get; set; }
        string GameCode { get; set; }
    }
}
