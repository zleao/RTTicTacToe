using System;
namespace RTTicTacToe.Forms.Messages
{
    public class GameFinishedMessage
    {
        public Guid GameId { get; }

        public GameFinishedMessage(Guid gameId)
        {
            GameId = gameId;
        }
    }
}
