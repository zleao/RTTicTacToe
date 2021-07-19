using System;
namespace RTTicTacToe.Forms.Messages
{
    public class PlayerJoinedGameMessage
    {
        public Guid GameId { get; }

        public PlayerJoinedGameMessage(Guid gameId)
        {
            GameId = gameId;
        }
    }
}
