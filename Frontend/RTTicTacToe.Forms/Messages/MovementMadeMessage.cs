using System;
namespace RTTicTacToe.Forms.Messages
{
    public class MovementMadeMessage
    {
        public Guid GameId { get; }

        public MovementMadeMessage(Guid gameId)
        {
            GameId = gameId;
        }
    }
}
