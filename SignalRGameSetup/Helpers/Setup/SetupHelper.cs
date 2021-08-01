using System;

namespace SignalRGameSetup.Helpers.Setup
{
    public static class SetupHelper
    {
        private static Random random = new Random();

        /// <summary>
        /// Generate a code that will allow users to join a particular game.
        /// </summary>
        /// <returns>Returns a 4 digit generated code.</returns>
        public static string GenerateGameCode()
        {
            string[] letters = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
            "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};
            int[] numbers = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };



        }

        public static bool GameCodeAvailable(string gameCode)
        {
            // TODO search database for code to see if it is taken
            return true;
        }

    }
}