using SignalRGameSetup.Logic;
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

            string gameCode = "";

            do
            {
                for (int i = 0; i < GameInformation.GameCodeLength; i++)
                {
                    // first decide whether to generate a letter or number
                    int decision = random.Next(0, 2);
                    string newCharacter;
                    // grab random character
                    if (decision == 0)
                    {
                        newCharacter = letters[random.Next(0, letters.Length)];
                    }
                    else
                    {
                        newCharacter = numbers[random.Next(0, numbers.Length)].ToString();
                    }
                    // add it to the code
                    gameCode += newCharacter;
                }

            } while (!GameCodeAvailable(gameCode));

            // once an available code has been generated, return it
            return gameCode;
        }


        /// <summary>
        /// Check the database as to whether a given game code is not already in use.
        /// </summary>
        /// <param name="gameCode"></param>
        /// <returns>Returns true if a game code is not taken, false if it is.</returns>
        public static bool GameCodeAvailable(string gameCode)
        {

            if (gameCode == null || gameCode.Length < GameInformation.GameCodeLength)
            {
                return false;
            }

            // TODO search database for code to see if it is taken
            return true;
        }

    }
}