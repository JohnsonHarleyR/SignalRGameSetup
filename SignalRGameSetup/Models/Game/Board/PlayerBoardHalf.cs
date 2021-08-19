using Newtonsoft.Json;
using SignalRGameSetup.Database.Dtos.Game;
using SignalRGameSetup.Helpers.Game;
using SignalRGameSetup.Models.Game.Board.Pieces;
using SignalRGameSetup.Models.Game.Interfaces;
using System.Collections.Generic;

namespace SignalRGameSetup.Models.Game.Board
{
    public class PlayerBoardHalf : IBoardHalf
    {
        public int BoardId { get; set; }
        public string ParticipantId { get; set; }
        public string GameCode { get; set; }
        public Dictionary<string, BoardPosition> Positions { get; set; }
        public Dictionary<string, ShipPiece> Ships { get; set; }
        public int ShipsLeft
        {
            get
            {
                // loop through the ships and count how many are not sunk
                int count = 0;
                foreach (var ship in Ships)
                {
                    if (!ship.Value.IsSunk)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public PlayerBoardHalf()
        {
            Positions = BoardHelper.CreatePositions();
            Ships = BoardHelper.CreateShips();
        }

        public PlayerBoardHalf(string gameCode, string participantId)
        {
            GameCode = gameCode;
            ParticipantId = participantId;
            Positions = BoardHelper.CreatePositions();
            Ships = BoardHelper.CreateShips();
        }

        public PlayerBoardHalf(PlayerBoardHalfDto dto)
        {
            BoardId = dto.BoardId;
            ParticipantId = dto.ParticipantId;
            GameCode = dto.GameCode;
            Positions = JsonConvert.DeserializeObject<Dictionary<string, BoardPosition>>(dto.Positions);
            Ships = JsonConvert.DeserializeObject<Dictionary<string, ShipPiece>>(dto.Ships);
        }

    }
}