using SignalRGameSetup.Database.Dtos;
using SignalRGameSetup.Models.Game.Interfaces;

namespace SignalRGameSetup.Models.Game.Board
{
    public class FullBoard : IBoard
    {
        public string GameCode { get; set; }
        public GuessBoardHalf EnemyBoard { get; set; }
        public PlayerBoardHalf PlayerBoard { get; set; }

        public FullBoard(string gameCode)
        {
            GameCode = gameCode;
            EnemyBoard = new GuessBoardHalf();
            PlayerBoard = new PlayerBoardHalf();
        }

        public FullBoard(BattleShipsDto dto, string playerParticipantId)
        {
            GameCode = dto.GameCode;

            string playerId;
            string enemyId;
            int playerBoardId;
            int enemyBoardId;
            // determine which player is which
            if (playerParticipantId == dto.PlayerOne)
            {
                playerId = dto.PlayerOne;
                playerBoardId = dto.PlayerOneBoard;
                enemyId = dto.PlayerTwo;
                enemyBoardId = dto.PlayerTwoBoard;
            }
            else
            {
                playerId = dto.PlayerTwo;
                playerBoardId = dto.PlayerTwoBoard;
                enemyId = dto.PlayerOne;
                enemyBoardId = dto.PlayerOneBoard;
            }

            // HACK Be sure to locate the boards in the database to set them here
            EnemyBoard = new GuessBoardHalf(GameCode, enemyId);
            PlayerBoard = new PlayerBoardHalf(GameCode, playerId);

            // TODO set up the boards according to who is who

            // user the GameHelper to reach into the database and grab both player board
            // then set the enemy and player board according to who is who
        }

    }

}