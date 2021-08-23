using SignalRGameSetup.Database.Dtos;
using SignalRGameSetup.Database.Repositories;
using SignalRGameSetup.Enums.Game;
using SignalRGameSetup.Models.Game.Board;
using SignalRGameSetup.Models.Game.Board.Pieces;
using System.Collections.Generic;

namespace SignalRGameSetup.Helpers.Game
{
    public static class BoardHelper
    {
        private static int boardSize = 10;
        private static readonly int[] SHIP_SIZES = new int[] { 5, 4, 3, 3, 2 }; // how big each ship is
        private static readonly string[] SHIP_NAMES = new string[]{"Carrier", // the names of each ship, matches respectively to sizes
        "Battleship", "Cruiser", "Submarine", "Destroyer"};

        public static int GetBoardSize()
        {
            return boardSize;
        }

        public static Dictionary<string, ShipPiece> CreateShips()
        {
            Dictionary<string, ShipPiece> ships = new Dictionary<string, ShipPiece>();

            for (int i = 0; i < SHIP_NAMES.Length; i++)
            {
                ships.Add(SHIP_NAMES[i], new ShipPiece(SHIP_NAMES[i], SHIP_SIZES[i]));
            }

            return ships;

        }

        public static Dictionary<string, BoardPosition> CreatePositions()
        {
            Dictionary<string, BoardPosition> positions = new Dictionary<string, BoardPosition>();

            // create all positions in the dictionary
            for (int r = 1; r <= BoardHelper.GetBoardSize(); r++)
            {
                for (int c = 1; c <= BoardHelper.GetBoardSize(); c++)
                {
                    BoardPosition newPosition = new BoardPosition((YPosition)r, c);
                    positions.Add(newPosition.Name, newPosition);
                }
            }
            return positions;

        }

        // NOTE this is based on who the active user is who is retrieving the board
        public static FullBoard GetFullBoardFromGameDto(string participantId, BattleShipsGameDto dto)
        {
            BoardRepository boardRepo = new BoardRepository();
            FullBoard fullBoard = new FullBoard(dto.GameCode);

            // stuff here
            string playerId;
            string enemyId;
            int playerBoardId;
            int enemyBoardId;
            // determine which player is which
            if (participantId == dto.PlayerOne)
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

            // Get player boards based on information and store it accordingly
            PlayerBoardHalf enemyPlayerBoard = boardRepo.GetPlayerBoardById(enemyBoardId);
            fullBoard.EnemyBoard = new GuessBoardHalf(enemyPlayerBoard);
            fullBoard.PlayerBoard = boardRepo.GetPlayerBoardById(playerBoardId);

            return fullBoard;
        }

        // This takes a guess board and updates the information on the corresponding player board
        public static PlayerBoardHalf GetUpdatedPlayerBoardFromGuessBoard(GuessBoardHalf guessBoard)
        {
            BoardRepository repo = new BoardRepository();

            // First grab the corresponding player board from the repo
            PlayerBoardHalf playerBoard =
                repo.GetPlayerBoardByInfo(guessBoard.GameCode, guessBoard.ParticipantId);

            if (playerBoard == null)
            {
                playerBoard = new PlayerBoardHalf(guessBoard.GameCode, guessBoard.ParticipantId);
            }

            // start updating
            playerBoard.Positions = guessBoard.Positions;

            // loop through ships in the player board and update them according to positions
            foreach (var ship in playerBoard.Ships.Values)
            {
                // loop through positions on ship - if not sunk
                if (ship.IsSet && !ship.IsSunk)
                {
                    // update the ship values
                    foreach (var position in ship.Positions)
                    {
                        var guessPosition = guessBoard.Positions[position.Key];
                        if (guessPosition.HasPeg)
                        {
                            position.Value.IsHit = true;
                        }
                    }

                    // if the ship has no hits left, set it to sunk
                    if (ship.HitsLeft == 0)
                    {
                        ship.IsSunk = true;
                    }
                }
            }

            // return it
            return playerBoard;

        }



    }
}