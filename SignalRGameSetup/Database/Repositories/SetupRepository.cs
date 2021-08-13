using Dapper;
using SignalRGameSetup.Database.Dtos;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace SignalRGameSetup.Database.Repositories
{
    public class SetupRepository
    {
        private string Schema = @"[dbo]";
        private string ConnectionString;

        public SetupRepository()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["SignalRGame"].ConnectionString;
        }

        public IEnumerable<GameSetupDto> GetAllSetups()
        {
            IEnumerable<GameSetupDto> setups;

            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = $"{Schema}.GetAllGameSetups";

                setups = connection.Query<GameSetupDto>(sql, commandType: System.Data.CommandType.StoredProcedure);

            }
            return setups;
        }

        public GameSetupDto GetSetupByGameCode(string gameCode)
        {
            GameSetupDto setup;

            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = $"{Schema}.GetSetupByGameCode";

                setup = connection.Query<GameSetupDto>(sql,
                    new { GameCode = gameCode },
                    commandType: System.Data.CommandType.StoredProcedure)?.FirstOrDefault();

            }
            return setup;
        }

        public void AddGameSetup(GameSetupDto setupDto)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = $"{Schema}.AddGameSetup";

                connection.Execute(sql,
                                    new
                                    {
                                        GameCode = setupDto.GameCode,
                                        Players = setupDto.Players,
                                        Watchers = setupDto.Watchers,
                                        AllowAudience = setupDto.AllowAudience,
                                        LeaveInDatabase = setupDto.LeaveInDatabase
                                    },
                                    commandType: System.Data.CommandType.StoredProcedure);

            }
        }

        public void UpdateGameSetup(GameSetupDto setupDto)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = $"{Schema}.UpdateGameSetup";

                connection.Execute(sql,
                                    new
                                    {
                                        GameCode = setupDto.GameCode,
                                        Players = setupDto.Players,
                                        Watchers = setupDto.Watchers,
                                        AllowAudience = setupDto.AllowAudience,
                                        LeaveInDatabase = setupDto.LeaveInDatabase
                                    },
                                    commandType: System.Data.CommandType.StoredProcedure);

            }
        }

        public void DeleteGameSetup(string gameCode)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = $"{Schema}.DeleteGameSetup";

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