using Newtonsoft.Json;
using SignalRGameSetup.Database.Dtos;
using SignalRGameSetup.Database.Repositories;
using SignalRGameSetup.Models.Game;
using SignalRGameSetup.Models.Game.Board;

namespace SignalRGameSetup.Helpers.Game
{
    public static class GameHelper
    {

        public static void DeleteGame(string gameCode)
        {
            GameRepository gameRepo = new GameRepository();
            gameRepo.DeleteBattleShipsGame(gameCode);
        }

        public static bool AddGame(BattleShipsGame game)
        {
            if (game == null)
            {
                return false;
            }

            GameRepository gameRepo = new GameRepository();
            BoardRepository boardRepo = new BoardRepository();


            // also save the player boards
            if (game.Board.PlayerBoard.BoardId == null)
            {
                boardRepo.AddPlayerBoard(game.Board.PlayerBoard);
                PlayerBoardHalf tempPlayerBoard =
                    boardRepo.GetPlayerBoardByInfo(game.Board.PlayerBoard.GameCode, game.Board.PlayerBoard.ParticipantId);
                game.Board.PlayerBoard = tempPlayerBoard;
            }
            if (game.Board.EnemyBoard.BoardId == null)
            {
                PlayerBoardHalf enemyBoard =
                    BoardHelper.GetUpdatedPlayerBoardFromGuessBoard(game.Board.EnemyBoard);
                boardRepo.AddPlayerBoard(enemyBoard);
                PlayerBoardHalf tempEnemyBoard =
    boardRepo.GetPlayerBoardByInfo(game.Board.EnemyBoard.GameCode, game.Board.EnemyBoard.ParticipantId);
                game.Board.EnemyBoard = new GuessBoardHalf(tempEnemyBoard);
            }

            // store info in a new dto
            BattleShipsGameDto dto = new BattleShipsGameDto()
            {
                GameCode = game.GameCode,
                PlayerOne = game.Board.PlayerBoard.ParticipantId,
                PlayerOneBoard = (int)game.Board.PlayerBoard.BoardId,
                PlayerTwo = game.Board.EnemyBoard.ParticipantId,
                PlayerTwoBoard = (int)game.Board.EnemyBoard.BoardId,
                Information = JsonConvert.SerializeObject(game.Information)
            };

            gameRepo.AddBattleShipsGame(dto);

            return true;
        }

        public static BattleShipsGame GetGameFromInfo(string gameCode, string participantId)
        {
            GameRepository gameRepo = new GameRepository();
            BoardRepository boardRepo = new BoardRepository();

            BattleShipsGameDto gameDto = gameRepo.GetBattleShipsGameByGameCode(gameCode);

            if (gameDto == null)
            {
                return null;
            }

            BattleShipsGame game = new BattleShipsGame(gameCode);

            // DO STUFF HERE

            PlayerBoardHalf playerBoard;
            GuessBoardHalf enemyBoard;

            if (participantId == gameDto.PlayerOne)
            {
                playerBoard = boardRepo.GetPlayerBoardById(gameDto.PlayerOneBoard);
                PlayerBoardHalf enemyPlayerBoard = boardRepo.GetPlayerBoardById(gameDto.PlayerTwoBoard);
                enemyBoard = new GuessBoardHalf(enemyPlayerBoard);

            }
            else if (participantId == gameDto.PlayerTwo)
            {
                playerBoard = boardRepo.GetPlayerBoardById(gameDto.PlayerTwoBoard);
                PlayerBoardHalf enemyPlayerBoard = boardRepo.GetPlayerBoardById(gameDto.PlayerOneBoard);
                enemyBoard = new GuessBoardHalf(enemyPlayerBoard);
            }
            else
            {
                return null;
            }

            // add boards to game
            FullBoard fullBoard = new FullBoard(gameCode);
            fullBoard.EnemyBoard = enemyBoard;
            fullBoard.PlayerBoard = playerBoard;
            game.Board = fullBoard;

            // add information
            game.Information = JsonConvert.DeserializeObject<BattleShipsInfo>(gameDto.Information);

            return game;
        }

        // Update the participant boards as well as the game information in the databases
        public static bool UpdateGame(BattleShipsGame game)
        {
            GameRepository gameRepo = new GameRepository();
            BoardRepository boardRepo = new BoardRepository();

            BattleShipsGameDto gameDto = gameRepo.GetBattleShipsGameByGameCode(game.GameCode);
            // update the information
            gameDto.Information = JsonConvert.SerializeObject(game.Information);

            if (gameDto == null)
            {
                return false;
            }

            // update the player boards.
            PlayerBoardHalf enemyBoard = BoardHelper.GetUpdatedPlayerBoardFromGuessBoard(game.Board.EnemyBoard);
            PlayerBoardHalf playerBoard = game.Board.PlayerBoard;
            boardRepo.UpdatePlayerBoard(enemyBoard);
            boardRepo.UpdatePlayerBoard(playerBoard);

            // update the game dto
            gameRepo.UpdateBattleShipsGame(gameDto);

            return true;
        }

    }
}