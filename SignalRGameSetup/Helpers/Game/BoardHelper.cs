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

        // This takes a guess board and updates the information on the corresponding player board
        public static PlayerBoardHalf UpdatePlayerBoardFromGuessBoard(GuessBoardHalf guessBoard)
        {
            BoardRepository repo = new BoardRepository();

            // First grab the corresponding player board from the repo
            PlayerBoardHalf playerBoard =
                repo.GetPlayerBoardByInfo(guessBoard.ParticipantId, guessBoard.GameCode);

            if (playerBoard == null)
            {
                return null;
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

            // now update the player board and then return it
            repo.UpdatePlayerBoard(playerBoard);
            return playerBoard;

        }

    }
}