using System;
using CQRSlite.Events;

namespace RTTicTacToe.CQRS.ReadModel.Events
{
    public class MovementMade : IEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public Guid PlayerId { get; set; }
        public int PlayerNumber { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public MovementMade(Guid id, Guid playerId, int playerNumber, int x, int y) 
        {
            Id = id;
            PlayerId = playerId;
            PlayerNumber = playerNumber;
            X = x;
            Y = y;
        }
    }
}
