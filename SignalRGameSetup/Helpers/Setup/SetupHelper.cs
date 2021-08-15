using SignalRGameSetup.Database.Dtos;
using SignalRGameSetup.Database.Repositories;
using SignalRGameSetup.Logic;
using SignalRGameSetup.Models.Setup;
using SignalRGameSetup.Models.Setup.Interfaces;
using System;
using System.Collections.Generic;

namespace SignalRGameSetup.Helpers.Setup
{

    // TODO add full description to all methods, including parameter info
    public static class SetupHelper
    {
        private static Random random = new Random();

        // overloaded method
        public static IParticipant GetParticipantById(string gameCode, string participantId)
        {
            // get the setup based on the game code
            GameSetup setup = SetupHelper.GetSetupByGameCode(gameCode);

            // if the setup is null, either the setup is still being created or it is
            // an invalid code. Either way, return null.
            if (setup == null)
            {
                return null;
            }

            IParticipant participant = null;

            // first look through the players
            foreach (var player in setup.Players)
            {
                if (player.ParticipantId == participantId)
                {
                    // if it matches, grab the player
                    participant = player;
                    break;
                }
            }

            // if participant is still null, check the watchers
            if (participant == null)
            {
                foreach (var watcher in setup.Watchers)
                {
                    if (watcher.ParticipantId == participantId)
                    {
                        // if it matches, grab the watcher
                        participant = watcher;
                        break;
                    }
                }
            }

            // return the result
            return participant;

        }

        public static IParticipant GetParticipantByConnectionId(string gameCode, string participantId)
        {
            // get the setup based on the game code
            GameSetup setup = SetupHelper.GetSetupByGameCode(gameCode);

            IParticipant participant = null;

            // first look through the players
            foreach (var player in setup.Players)
            {
                if (player.ConnectionId == participantId)
                {
                    // if it matches, grab the player
                    participant = player;
                    break;
                }
            }

            // if participant is still null, check the watchers
            if (participant == null)
            {
                foreach (var watcher in setup.Watchers)
                {
                    if (watcher.ConnectionId == participantId)
                    {
                        // if it matches, grab the watcher
                        participant = watcher;
                        break;
                    }
                }
            }

            // return the result
            return participant;

        }

        // Get a participant based on a connection id
        public static IParticipant GetParticipantByConnectionId(string connectionId)
        {
            // first get a list of all setups
            List<GameSetup> setups = GetAllSetups();

            // loop through setups - if it finds the connection id, set the participant and return
            IParticipant participant = null;
            foreach (var setup in setups)
            {
                participant = GetParticipantByConnectionId(setup.GameCode, connectionId);
                if (participant != null)
                {
                    break;
                }
            }

            return participant;
        }

        public static GameSetup AddGameSetup(GameSetup setup)
        {
            SetupRepository repository = new SetupRepository();


            repository.AddGameSetup(new GameSetupDto(setup));

            return setup;
        }

        public static List<GameSetup> GetAllSetups()
        {
            SetupRepository repository = new SetupRepository();

            IEnumerable<GameSetupDto> setupDtos = repository.GetAllSetups();
            List<GameSetup> setups = new List<GameSetup>();
            foreach (var dto in setupDtos)
            {
                setups.Add(new GameSetup(dto));
            }

            return setups;
        }

        public static void UpdateGameSetup(GameSetup setup)
        {
            SetupRepository repository = new SetupRepository();
            repository.UpdateGameSetup(new GameSetupDto(setup));
        }

        public static GameSetup GetSetupByGameCode(string gameCode)
        {
            SetupRepository repository = new SetupRepository();
            GameSetupDto setupDto = repository.GetSetupByGameCode(gameCode);
            if (setupDto != null)
            {
                GameSetup setup = new GameSetup(setupDto);
                setup.CalculateAvailable(); // update how many players and watchers are available
                return setup;
            }
            else
            {
                return null;
            }
        }

        public static void DeleteGameSetup(string gameCode)
        {
            SetupRepository repository = new SetupRepository();
            repository.DeleteGameSetup(gameCode);
        }

        /// <summary>
        /// Generate a code with a given length of characters.
        /// </summary>
        /// <returns>Returns a generated code.</returns>
        public static string GenerateCode(int length)
        {
            string[] letters = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
            "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};
            int[] numbers = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

            string code = "";

            for (int i = 0; i < length; i++)
            {
                // first decide whether to generate a letter or number
                int decision = random.Next(0, 3);
                string newCharacter;
                // grab random character
                if (decision == 0)
                {
                    newCharacter = letters[random.Next(0, letters.Length)];
                }
                else if (decision == 1)
                {
                    newCharacter = numbers[random.Next(0, numbers.Length)].ToString();
                }
                else
                {
                    newCharacter = letters[random.Next(0, letters.Length)].ToLower();
                }
                // add it to the code
                code += newCharacter;
            }

            // once an available code has been generated, return it
            return code;
        }

        /// <summary>
        /// Generate a code that will allow users to join a particular game.
        /// </summary>
        /// <returns>Returns a generated code with a specified number of characters.</returns>
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

            SetupRepository repository = new SetupRepository();
            GameSetupDto setupDto = repository.GetSetupByGameCode(gameCode);

            if (setupDto != null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Generate a code that will allow users to join a particular game. This one does not check for existing.
        /// </summary>
        /// <returns>Returns a generated code with a specified number of characters.</returns>
        public static string GenerateParticipantId()
        {

            string[] letters = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
            "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};
            int[] numbers = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

            string participantCode = "";


            for (int i = 0; i < GameInformation.ParticipantCodeLength; i++)
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
                participantCode += newCharacter;
            }

            return participantCode;
        }

        /// <summary>
        /// Generate a code that will allow users to join a particular game. This overloaded method checks if an Id is available in a game.
        /// </summary>
        /// <returns>Returns a generated code with a specified number of characters.</returns>
        public static string GenerateParticipantId(string gameCode)
        {
            // TODO Consider doing something similar for the participant's name - will probably have to use AJAX, I think.

            string[] letters = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
            "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};
            int[] numbers = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

            string participantCode = "";

            do
            {

                for (int i = 0; i < GameInformation.ParticipantCodeLength; i++)
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
                    participantCode += newCharacter;
                }

            } while (!ParticipantIdAvailable(gameCode, participantCode));

            return participantCode;
        }

        /// <summary>
        /// Check the database as to whether a given game code is not already in use.
        /// </summary>
        /// <param name="gameCode"></param>
        /// <param name="participantId"></param>
        /// <returns>Returns true if a game code is not taken, false if it is.</returns>
        public static bool ParticipantIdAvailable(string gameCode, string participantId)
        {
            // return false if either parameter is null or impossible
            if (participantId == null || participantId.Length < GameInformation.ParticipantCodeLength)
            {
                return false;
            }

            IParticipant participant = SetupHelper.GetParticipantById(gameCode, participantId);

            // if the method was able to grab a participant, that means the Id is not available.
            if (participant != null)
            {
                return false;
            }

            // If it came up with nothing, the id is available.
            return true;
        }

    }
}