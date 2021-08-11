using Microsoft.AspNet.SignalR;
using SignalRGameSetup.Database.Dtos;
using SignalRGameSetup.Database.Repositories;
using SignalRGameSetup.Helpers.Chat;
using SignalRGameSetup.Models.Chat.Containers;

namespace SignalRGameSetup.Hubs
{
    public class ChatHub : Hub
    {
        public void AddToChatGroup(Participant participant)
        {
            Groups.Add(Context.ConnectionId, participant.GameCode);

            // after adding, load the chat and add string saying the user has joined
            // also set that save option to true so it updates for everyone else too
            GameChat chat = ChatHelper.GetChatByGameCode(participant.GameCode);
            chat.DoSaveAfterShow = true;

            // get string to add to chat
            string noticeToAdd = ChatHelper
                .GetNoticeString($"{participant.Name} has joined the chat!", "green");
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
        public void LoadGameChatBeforeNotice(string gameCode)
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

            Clients.Caller.loadTheChatBeforeAdd(chat);

        }

        // call this one after loading the page when entering the wait or game room
        public void LoadGameChat(string gameCode)
        {
            GameChat chat = ChatHelper.GetChatByGameCode(gameCode);

            Clients.Group(gameCode).loadTheChat(chat);

        }

    }
}