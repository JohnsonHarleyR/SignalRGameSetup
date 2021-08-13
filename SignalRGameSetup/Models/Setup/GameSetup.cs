using Newtonsoft.Json;
using SignalRGameSetup.Database.Dtos;
using SignalRGameSetup.Helpers.Setup;
using SignalRGameSetup.Logic;
using System.Collections.Generic;
using System.Linq;

namespace SignalRGameSetup.Models.Setup
{
    public class GameSetup
    {

        // Each object instance is its own game.

        public string GameCode { get; set; } // Code that gets generated in order to join a game - used as primary key in DB
        public List<Player> Players { get; } // You must use the AddPlayer() method to add players
        public List<Watcher> Watchers { get; } // You must use the AddWatcher() method to add watchers
        public bool AllowAudience { get; set; }
        public bool LeaveInDatabase { get; set; } // this is for navigating between the wait room and game room so nothing gets deleted in the process

        public int PlayersAvailableToJoin { get; set; } // how many players are still available to join
        public int WatchersAvailableToJoin { get; set; } // how many players are still available to join

        //public IParticipant ActiveParticipant { get; set; } // the active player

        public GameSetup()
        {
            // Generate random game code to allow users to join
            GameCode = SetupHelper.GenerateGameCode();
            Players = new List<Player>();
            Watchers = new List<Watcher>();
            LeaveInDatabase = false;
        }

        public GameSetup(bool allowAudience)
        {
            // Generate random game code to allow users to join
            GameCode = SetupHelper.GenerateGameCode();
            Players = new List<Player>();
            Watchers = new List<Watcher>();
            AllowAudience = allowAudience;
            LeaveInDatabase = false;
        }

        public GameSetup(GameSetupDto setupDto)
        {
            GameCode = setupDto.GameCode;

            Players = JsonConvert.DeserializeObject<List<Player>>(setupDto.Players);
            Watchers = JsonConvert.DeserializeObject<List<Watcher>>(setupDto.Watchers);
            AllowAudience = setupDto.AllowAudience;
            LeaveInDatabase = setupDto.LeaveInDatabase;

            // update how many are available
            CalculateAvailable();
        }


        /// <summary>
        /// This will add a player if the maximum number of players has not been reached and if the name and connectionId are not null.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="connectionId"></param>
        /// <returns>Returns true or false based on whether a player was successfully added.</returns>
        public bool AddPlayer(Player player)
        {

            if (Players.Count >= GameInformation.MaximumPlayers || player == null ||
                player.Name == null || player.ConnectionId == null)
            {
                return false;
            }

            Players.Add(player);

            // update how many are available
            CalculateAvailable();

            return true;
        }

        /// <summary>
        /// This will remove a player from the list of players.
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns>Returns true or false based on whether a player was successfully removed.</returns>
        public bool RemovePlayer(string connectionId)
        {

            if (connectionId == null)
            {
                return false;
            }

            // find the user in the list
            Player player = Players.Where(p => p.ConnectionId == connectionId).FirstOrDefault();

            // if it's null, it's unsuccessful so return false
            if (player == null)
            {
                return false;
            }

            Players.Remove(player);

            // update how many are available
            CalculateAvailable();

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

            // update how many are available
            CalculateAvailable();

            return true;
        }

        /// <summary>
        /// This will remove a watcher from the list of watchers.
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns>Returns true or false based on whether a watcher was successfully removed.</returns>
        public bool RemoveWatcher(string connectionId)
        {

            if (connectionId == null)
            {
                return false;
            }

            // find the user in the list
            Watcher watcher = Watchers.Where(w => w.ConnectionId == connectionId).FirstOrDefault();

            // if it's null, it's unsuccessful so return false
            if (watcher == null)
            {
                return false;
            }

            Watchers.Remove(watcher);

            // update how many are available
            CalculateAvailable();

            return true;
        }

        /// <summary>
        /// Calculate how many players can still join and how many watchers.
        /// </summary>
        public void CalculateAvailable()
        {
            // get max players and max watchers
            int maxPlayers = GameInformation.MaximumPlayers;
            int maxWatchers = 0;
            if (AllowAudience)
            {
                maxWatchers = GameInformation.MaximumWatchers;
            }

            // get current players and watchers
            int currentPlayers = Players.Count;
            int currentWatchers = Watchers.Count;

            // calculate how many are available.
            PlayersAvailableToJoin = maxPlayers - currentPlayers;
            WatchersAvailableToJoin = maxWatchers - currentWatchers;

            // make sure nothing is below 0
            if (PlayersAvailableToJoin < 0)
            {
                PlayersAvailableToJoin = 0;
            }
            if (WatchersAvailableToJoin < 0)
            {
                WatchersAvailableToJoin = 0;
            }
        }


    }
}