using Dapper;
using SignalRGameSetup.Database.Dtos;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace SignalRGameSetup.Database.Repositories
{
    public class GameRepository
    {
        private string Schema = @"[dbo]";
        private string ConnectionString;

        public GameRepository()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["BattleShips"].ConnectionString;
        }

        public void AddBattleShipsGame(BattleShipsGameDto dto)
        {


            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = $"{Schema}.AddBattleShipsGame";

                connection.Execute(sql,
                                    new
                                    {
                                        GameCode = dto.GameCode,
                                        PlayerOne = dto.PlayerOne,
                                        PlayerOneBoard = dto.PlayerOneBoard,
                                        PlayerTwo = dto.PlayerTwo,
                                        PlayerTwoBoard = dto.PlayerTwoBoard,
                                        Information = dto.Information
                                    },
                                    commandType: System.Data.CommandType.StoredProcedure);

            }
        }

        public void UpdateBattleShipsGame(BattleShipsGameDto dto)
        {


            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = $"{Schema}.UpdateBattleShipsGame";

                connection.Execute(sql,
                                    new
                                    {
                                        GameCode = dto.GameCode,
                                        PlayerOne = dto.PlayerOne,
                                        PlayerOneBoard = dto.PlayerOneBoard,
                                        PlayerTwo = dto.PlayerTwo,
                                        PlayerTwoBoard = dto.PlayerTwoBoard,
                                        Information = dto.Information
                                    },
                                    commandType: System.Data.CommandType.StoredProcedure);

            }
        }


        public BattleShipsGameDto GetBattleShipsGameByGameCode(string gameCode)
        {
            BattleShipsGameDto dto;

            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = $"{Schema}.GetBattleShipsGameByGameCode";

                dto = connection.Query<BattleShipsGameDto>(sql,
                    new
                    {
                        GameCode = gameCode
                    },
                    commandType: System.Data.CommandType.StoredProcedure)?.FirstOrDefault();

            }

            return dto;
        }


        public IEnumerable<BattleShipsGameDto> GetAllBattleShipGames()
        {
            IEnumerable<BattleShipsGameDto> dtos;

            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = $"{Schema}.GetAllBattleShipGames";

                dtos = connection.Query<BattleShipsGameDto>(sql, commandType: System.Data.CommandType.StoredProcedure);

            }

            return dtos;
        }

        public void DeleteBattleShipsGame(string gameCode)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = $"{Schema}.DeleteBattleShipsGame";

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