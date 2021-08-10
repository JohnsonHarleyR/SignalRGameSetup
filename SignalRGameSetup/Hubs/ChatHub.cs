using Microsoft.AspNet.SignalR;
using SignalRGameSetup.Database.Dtos;
using SignalRGameSetup.Database.Repositories;
using SignalRGameSetup.Models.Chat.Containers;

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

        // save the chat to a database for later
        public void SaveGameChat(GameChat chat)
        {
            // create a repo
            ChatRepository chatRepo = new ChatRepository();

            // see if the chat exists already or not - try to retrieve it
            GameChatDto chatDto = chatRepo.GetChatByGameCode(chat.GameCode);

            // if it's null, create a new item and save it
            if (chatDto == null)
            {
                chatDto = new GameChatDto()
                {
                    GameCode = chat.GameCode,
                    ChatHtml = chat.ChatHtml
                };

                chatRepo.AddGameChat(chatDto);
            }
            else // otherwise update it
            {
                chatDto.ChatHtml = chat.ChatHtml;

                chatRepo.UpdateGameChat(chatDto);
            }

        }

        // call this one after loading the page when entering the wait or game room
        public void LoadGameChat(string gameCode)
        {
            // get the correct chat
            ChatRepository chatRepo = new ChatRepository();
            GameChatDto chatDto = chatRepo.GetChatByGameCode(gameCode);

            // if it comes back null, create a new one and save it to repo
            if (chatDto == null)
            {
                chatDto = new GameChatDto()
                {
                    GameCode = gameCode,
                    ChatHtml = ""
                };
            }

            // save it as a regular GameChat and save it
            GameChat chat = new GameChat()
            {
                GameCode = chatDto.GameCode,
                ChatHtml = chatDto.ChatHtml
            };

            Clients.Client(Context.ConnectionId).loadChat();

        }

    }
}