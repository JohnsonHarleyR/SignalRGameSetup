using Microsoft.AspNet.SignalR;
using SignalRGameSetup.Helpers.Chat;
using SignalRGameSetup.Helpers.Setup;
using SignalRGameSetup.Models.Chat.Containers;
using SignalRGameSetup.Models.Setup.Interfaces;
using System.Threading.Tasks;

namespace SignalRGameSetup.Hubs
{
    public class ChatHub : Hub
    {

        public override Task OnDisconnected(bool stopCalled)
        {
            if (stopCalled)
            {
                //We know that Stop() was called on the client,
                //and the connection shut down gracefully.

                // get the game code and participant's id from cookies - if they exist
                string gameCode = null;
                string participantId = null;

                var httpContext = Context.Request.GetHttpContext();

                if (httpContext.Request.Cookies["GameCode"] != null)
                {
                    gameCode = httpContext.Request.Cookies["GameCode"].Value;
                }
                if (httpContext.Request.Cookies["ParticipantId"] != null)
                {
                    participantId = httpContext.Request.Cookies["ParticipantId"].Value;
                }

                // if the values are not null, add notice saying the player has left the chat
                if (gameCode != null && participantId != null)
                {

                    // grab the participant by their ID
                    IParticipant participant = SetupHelper.GetParticipantById(gameCode, participantId);

                    // get the chat based on the game code
                    GameChat chat = ChatHelper.GetChatByGameCode(gameCode);

                    if (participant != null)
                    {
                        // add a notice saying the player has left
                        chat.ChatHtml += ChatHelper.GetNoticeString($"{participant.Name} has left the room!",
                            "red");

                        // now save the chat
                        ChatHelper.SaveChat(chat);

                        // now load the new chat for everyone
                        chat.DoSaveAfterShow = true;
                        Clients.Group(gameCode).loadTheChat(chat);
                    }

                }

            }
            else
            {
                // This server hasn't heard from the client in the last ~35 seconds.
                // If SignalR is behind a load balancer with scaleout configured, 
                // the client may still be connected to another SignalR server.
            }

            return base.OnDisconnected(stopCalled);
        }

        public void AddToChatGroup(Participant participant)
        {
            Groups.Add(Context.ConnectionId, participant.GameCode);

            // after adding, load the chat and add string saying the user has joined
            // also set that save option to true so it updates for everyone else too
            GameChat chat = ChatHelper.GetChatByGameCode(participant.GameCode);
            chat.DoSaveAfterShow = true;

            // get string to add to chat
            string noticeToAdd = ChatHelper
                .GetNoticeString($"{participant.Name} has joined the room!", "green");
            chat.ChatHtml += noticeToAdd;

            Clients.Client(Context.ConnectionId).addParticipantToChat(chat);
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


        // save the chat to a database for later
        public void SaveGameChat(GameChat chat)
        {
            ChatHelper.SaveChat(chat);

            Clients.Group(chat.GameCode).loadTheChat(chat);

        }

        // call this one after loading the page when entering the wait or game room
        public void LoadGameChat(string gameCode)
        {
            GameChat chat = ChatHelper.GetChatByGameCode(gameCode);

            Clients.Group(gameCode).loadTheChat(chat);

        }

    }
}