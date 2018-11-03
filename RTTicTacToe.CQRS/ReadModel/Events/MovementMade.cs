using System;
using CQRSlite.Events;

namespace RTTicTacToe.CQRS.ReadModel.Events
{
    public class MovementMade : IEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public Guid MovementId { get; set; }
        public Guid PlayerId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public MovementMade(Guid id, Guid movementId, Guid playerId, int x, int y) 
        {
            Id = id;
            MovementId = movementId;
            PlayerId = playerId;
            X = x;
            Y = y;
        }
    }
}
