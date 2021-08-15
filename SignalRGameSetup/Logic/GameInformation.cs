﻿namespace SignalRGameSetup.Logic
{
    // Enter game information here
    public static class GameInformation
    {
        public static string Title { get; } = "Battle Ships"; // It is recommended to keep this consistent if you choose to use this for multiple games
        public static string Description { get; } = "Game description goes here.";
        public static int MinimumPlayers { get; } = 1;
        public static int MaximumPlayers { get; } = 2; // Make the minimum and maximum the same if you want a set number
        public static int MaximumWatchers { get; } = 5; // How many people can join the game to watch?
        public static int GameCodeLength { get; } = 4; // This is the default
        public static int ParticipantCodeLength { get; } = 4; // This is the default
    }
}