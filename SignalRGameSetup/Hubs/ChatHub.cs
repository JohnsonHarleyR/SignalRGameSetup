using Microsoft.AspNet.SignalR;
using SignalRGameSetup.Models.Setup.Containers;

namespace SignalRGameSetup.Hubs
{
    public class ChatHub : Hub
    {
        public void AddToChatGroup(Participant participant)
        {
            Groups.Add(Context.ConnectionId, participant.GameCode);
            NewMessage message = new NewMessage()
            {
                Name = participant.Name,
                Message = $"{participant.Name} has joined the chat!",
                GameCode = participant.GameCode
            };
            Clients.Group(participant.GameCode/*, Context.ConnectionId*/).addToChat(message);
        }

        public void NewMessage(NewMessage message)
        {
            NewMessage newMessage = new NewMessage()
            {
                Name = message.Name,
                Message = message.Message,
                GameCode = message.GameCode
            };
            // send the message to all clients in group
            Clients.Group(message.GameCode).addNewMessage(newMessage);
            //Clients.Group(message.GameCode).testThis();

        }
    }
}