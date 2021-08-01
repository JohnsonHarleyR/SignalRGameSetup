using SignalRGameSetup.Logic;
using System.Collections.Generic;

namespace SignalRGameSetup.Models.Setup
{
    public class GameSetup
    {

        // Each object instance is its own game.

        public string GameCode { get; set; } // Code that gets generated in order to join a game - used as primary key in DB
        public List<Player> Players { get; } // You must use the AddPlayer() method to add players
        public List<Watcher> Watchers { get; } // You must use the AddWatcher() method to add watchers



        /// <summary>
        /// This will add a player if the maximum number of players has not been reached and if the name and connectionId are not null.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="connectionId"></param>
        /// <returns>Returns true or false based on whether a player was successfully added.</returns>
        public bool AddPlayer(string name, string connectionId)
        {

            if (Players.Count >= GameInformation.MaximumPlayers ||
                name == null || connectionId == null)
            {
                return false;
            }

            Player player = new Player()
            {
                Name = name,
                ConnectionId = connectionId
            };

            Players.Add(player);

            return true;
        }

        /// <summary>
        /// This will add a watcher if the maximum number of watchers has not been reached and if the name and connectionId are not null.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="connectionId"></param>
        /// <returns>Returns true or false based on whether a watcher was successfully added.</returns>
        public bool AddWatcher(string name, string connectionId)
        {

            if (Watchers.Count >= GameInformation.MaximumWatchers ||
                name == null || connectionId == null)
            {
                return false;
            }

            Watcher watcher = new Watcher()
            {
                Name = name,
                ConnectionId = connectionId
            };

            Watchers.Add(watcher);

            return true;
        }


    }
}