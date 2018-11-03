using System;
using CQRSlite.Events;

namespace RTTicTacToe.CQRS.ReadModel.Events
{
    public class PlayerRegistered : IEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public Guid PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int PlayerNumber { get; set; }

        public PlayerRegistered(Guid id, Guid playerId, string playerName, int playerNumber)
        {
            Id = id;
            PlayerId = playerId;
            PlayerName = playerName;
            PlayerNumber = playerNumber;
        }
    }
}
