using SignalRGameSetup.Database.Dtos;
using SignalRGameSetup.Database.Repositories;
using SignalRGameSetup.Logic;
using SignalRGameSetup.Models.Setup;
using SignalRGameSetup.Models.Setup.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

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
        /// Generate a code that will allow users to join a particular game.
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

            // TODO fix so the player id code can be successfully checked to see if it already exists
            //do
            //{
            //    for (int i = 0; i < GameInformation.ParticipantCodeLength; i++)
            //    {
            //        // first decide whether to generate a letter or number
            //        int decision = random.Next(0, 2);
            //        string newCharacter;
            //        // grab random character
            //        if (decision == 0)
            //        {
            //            newCharacter = letters[random.Next(0, letters.Length)];
            //        }
            //        else
            //        {
            //            newCharacter = numbers[random.Next(0, numbers.Length)].ToString();
            //        }
            //        // add it to the code
            //        participantCode += newCharacter;
            //    }

            //} while (!ParticipantIdAvailable(participantCode));

            // once an available code has been generated, return it
            return participantCode;
        }


        /// <summary>
        /// Check the database as to whether a given game code is not already in use.
        /// </summary>
        /// <param name="participantId"></param>
        /// <returns>Returns true if a game code is not taken, false if it is.</returns>
        public static bool ParticipantIdAvailable(string participantId)
        {
            // TODO fix this method

            if (participantId == null || participantId.Length < GameInformation.ParticipantCodeLength)
            {
                return false;
            }

            SetupRepository repository = new SetupRepository();
            IEnumerable<GameSetupDto> setupDtos = repository.GetAllSetups();
            List<GameSetup> setups = new List<GameSetup>();
            foreach (var setup in setupDtos)
            {
                setups.Add(new GameSetup(setup));
            }
            //setupDtos.ForEach(s => setups.Add(new GameSetup(s)));
            List<IParticipant> allParticipants = new List<IParticipant>();
            setups.ForEach(s => s.Players.ForEach(p => allParticipants.Add((IParticipant)p)));
            setups.ForEach(s => s.Watchers.ForEach(w => allParticipants.Add((IParticipant)w)));
            IParticipant participant = allParticipants.Where(p => p.ParticipantId == participantId).FirstOrDefault();

            if (participant != null)
            {
                return false;
            }

            return true;
        }

    }
}