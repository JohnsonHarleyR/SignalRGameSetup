﻿namespace SignalRGameSetup.Models.Chat.Containers
{
    public class GameChat
    {
        public string GameCode { get; set; }
        public string ChatHtml { get; set; }
        public bool DoSaveAfterShow { get; set; } = false;
    }
}