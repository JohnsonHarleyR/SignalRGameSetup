using SignalRGameSetup.Database.Dtos;
using SignalRGameSetup.Database.Repositories;
using SignalRGameSetup.Models.Chat.Containers;
using System;

namespace SignalRGameSetup.Helpers.Chat
{
    public class ChatHelper
    {

        // Get a string in html format to add as a notice to chat text
        // The color string should be in hex, rgb, or any relevent CSS color value
        public static string GetNoticeString(string message, string color)
        {
            string notice = $"<i style=\"color: {color};\">{message}</i><br>";
            return notice;
        }

        public static GameChat GetChatByGameCode(string gameCode)
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

            return chat;
        }

        // This will update a chat or create a new one - depending on whether one exists already
        public static bool SaveChat(GameChat chat)
        {
            try
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

                // return true if successful
                return true;

            }
            catch (Exception)
            {
                // return false if something goes wrong
                return false;
            }

        }

        public static void DeleteGameChat(string gameCode)
        {
            ChatRepository repository = new ChatRepository();
            repository.DeleteGameChat(gameCode);
        }

    }
}