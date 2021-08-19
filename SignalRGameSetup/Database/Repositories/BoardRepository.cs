using Dapper;
using Newtonsoft.Json;
using SignalRGameSetup.Database.Dtos.Game;
using SignalRGameSetup.Models.Game.Board;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace SignalRGameSetup.Database.Repositories
{
    public class BoardRepository
    {
        private string Schema = @"[dbo]";
        private string ConnectionString;

        public BoardRepository()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["BattleShips"].ConnectionString;
        }

        // PLAYER BOARD

        public void AddPlayerBoard(PlayerBoardHalf board)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = $"{Schema}.AddPlayerBoard";

                connection.Execute(sql,
                                    new
                                    {
                                        ParticipantId = board.ParticipantId,
                                        GameCode = board.GameCode,
                                        Positions = JsonConvert.SerializeObject(board.Positions),
                                        Ships = JsonConvert.SerializeObject(board.Ships)
                                    },
                                    commandType: System.Data.CommandType.StoredProcedure);

            }
        }

        public void UpdatePlayerBoard(PlayerBoardHalf board)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = $"{Schema}.UpdatePlayerBoard";

                connection.Execute(sql,
                                    new
                                    {
                                        BoardId = board.BoardId,
                                        ParticipantId = board.ParticipantId,
                                        GameCode = board.GameCode,
                                        Positions = JsonConvert.SerializeObject(board.Positions),
                                        Ships = JsonConvert.SerializeObject(board.Ships)
                                    },
                                    commandType: System.Data.CommandType.StoredProcedure);

            }
        }

        public PlayerBoardHalf GetPlayerBoardByInfo(string participantId, string gameCode)
        {
            PlayerBoardHalf boardHalf;
            PlayerBoardHalfDto dto;

            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = $"{Schema}.GetPlayerBoardByInfo";

                dto = connection.Query<PlayerBoardHalfDto>(sql,
                    new
                    {
                        ParticipantId = participantId,
                        GameCode = gameCode
                    },
                    commandType: System.Data.CommandType.StoredProcedure)?.FirstOrDefault();

            }

            boardHalf = new PlayerBoardHalf(dto);
            return boardHalf;
        }

        public PlayerBoardHalf GetPlayerBoardById(string boardId)
        {
            PlayerBoardHalf boardHalf;
            PlayerBoardHalfDto dto;

            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = $"{Schema}.GetPlayerBoardById";

                dto = connection.Query<PlayerBoardHalfDto>(sql,
                    new
                    {
                        BoardId = boardId
                    },
                    commandType: System.Data.CommandType.StoredProcedure)?.FirstOrDefault();

            }

            boardHalf = new PlayerBoardHalf(dto);
            return boardHalf;
        }


        public List<PlayerBoardHalf> GetAllPlayerBoards()
        {
            IEnumerable<PlayerBoardHalfDto> dtos;
            List<PlayerBoardHalf> boards = new List<PlayerBoardHalf>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = $"{Schema}.GetAllPlayerBoards";

                dtos = connection.Query<PlayerBoardHalfDto>(sql, commandType: System.Data.CommandType.StoredProcedure);

            }

            foreach (var dto in dtos)
            {
                boards.Add(new PlayerBoardHalf(dto));
            }

            return boards;
        }

        public void DeletePlayerBoard(int boardId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = $"{Schema}.DeletePlayerBoard";

                connection.Execute(sql,
                                    new
                                    {
                                        BoardId = boardId
                                    },
                                    commandType: System.Data.CommandType.StoredProcedure);

            }
        }


    }
}