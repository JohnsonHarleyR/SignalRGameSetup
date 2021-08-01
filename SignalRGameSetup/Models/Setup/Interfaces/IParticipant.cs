namespace SignalRGameSetup.Models.Setup.Interfaces
{
    public interface IParticipant
    {
        string Name { get; set; }
        string ConnectionId { get; set; }
    }
}
