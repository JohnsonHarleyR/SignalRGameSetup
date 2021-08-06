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
    public static class SetupHelper
    {
        private static Random random = new Random();

        //public static GameSetup NewGameSetup(Player player, bool allowAudience)
        //{
        //    SetupRepository repository = new SetupRepository();

        //    GameSetup setup = new GameSetup(allowAudience);
        //    setup.AddPlayer(player);

        //    repository.AddGameSetup(new GameSetupDto(setup));

        //    return setup;
        //}

        public static GameSetup AddGameSetup(GameSetup setup)
        {
            SetupRepository repository = new SetupRepository();


            repository.AddGameSetup(new GameSetupDto(setup));

            return setup;
        }

        public static void UpdateGameSetup(GameSetup setup)
        {
            SetupRepository repository = new SetupRepository();
            repository.UpdateGameSetup(new GameSetupDto(setup));
        }

        public static GameSetup GetSetupByGameCode(string gameCode)
        {
            SetupRepository repository = new SetupRepository();
            GameSetup setup = new GameSetup(repository.GetSetupByGameCode(gameCode));
            return setup;
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