﻿using SignalRGameSetup.Helpers.Game;
using SignalRGameSetup.Models.Game.Board.Pieces;
using SignalRGameSetup.Models.Game.Interfaces;
using System.Collections.Generic;

namespace SignalRGameSetup.Models.Game.Board
{
    public class GuessBoardHalf : IBoardHalf
    {
        // This should be basically the same as a player board except the ships are not identified
        public int? BoardId { get; set; }
        public string ParticipantId { get; set; }
        public string GameCode { get; set; }
        public Dictionary<string, BoardPosition> Positions { get; set; }
        private int _shipsLeft;
        public int ShipsLeft { get { return _shipsLeft; } }

        public GuessBoardHalf()
        {
            Positions = BoardHelper.CreatePositions();
        }
        public GuessBoardHalf(string gameCode, string participantId)
        {
            GameCode = gameCode;
            ParticipantId = participantId;
            Positions = BoardHelper.CreatePositions();
        }

        // This one will be useful for converting a player board into a guess board
        public GuessBoardHalf(PlayerBoardHalf board)
        {
            BoardId = board.BoardId;
            ParticipantId = board.ParticipantId;
            GameCode = board.GameCode;
            _shipsLeft = board.ShipsLeft;

            if (board.Positions == null)
            {
                Positions = BoardHelper.CreatePositions();
            }
            else
            {
                Positions = board.Positions;
            }

        }

    }
}