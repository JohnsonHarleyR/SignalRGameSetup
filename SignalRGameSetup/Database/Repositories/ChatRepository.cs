using Dapper;
using SignalRGameSetup.Database.Dtos;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace SignalRGameSetup.Database.Repositories
{
    public class ChatRepository
    {
        private string Schema = @"[dbo]";
        private string ConnectionString;

        public ChatRepository()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["SignalRGame"].ConnectionString;
        }

        public void AddGameChat(GameChatDto chatDto)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = $"{Schema}.AddGameChat";

                connection.Execute(sql,
                                    new
                                    {
                                        GameCode = chatDto.GameCode,
                                        ChatHtml = chatDto.ChatHtml
                                    },
                                    commandType: System.Data.CommandType.StoredProcedure);

            }
        }

        public GameChatDto GetChatByGameCode(string gameCode)
        {
            GameChatDto chat;

            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = $"{Schema}.GetChatByGameCode";

                chat = connection.Query<GameChatDto>(sql,
                    new { GameCode = gameCode },
                    commandType: System.Data.CommandType.StoredProcedure)?.FirstOrDefault();

            }
            return chat;
        }

        public void UpdateGameChat(GameChatDto chatDto)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = $"{Schema}.UpdateGameChat";

                connection.Execute(sql,
                                    new
                                    {
                                        GameCode = chatDto.GameCode,
                                        ChatHtml = chatDto.ChatHtml
                                    },
                                    commandType: System.Data.CommandType.StoredProcedure);

            }
        }

        public void DeleteGameChat(string gameCode)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = $"{Schema}.DeleteGameChat";

                connection.Execute(sql,
                                    new
                                    {
                                        GameCode = gameCode
                                    },
                                    commandType: System.Data.CommandType.StoredProcedure);

            }
        }


    }
}