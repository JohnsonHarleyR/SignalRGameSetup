﻿using SignalRGameSetup.Models.Game.Interfaces;

namespace SignalRGameSetup.Models.Game.Board
{
    public class EnemyBoardHalf : IBoardHalf
    {
        public string ParticipantId { get; set; }
        public int BoardSize { get; set; }

    }
}