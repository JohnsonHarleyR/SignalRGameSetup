using SignalRGameSetup.Enums.Game;
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

    }
}