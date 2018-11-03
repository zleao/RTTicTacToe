using System;
using CQRSlite.Events;

namespace RTTicTacToe.CQRS.ReadModel.Events
{
    public class GameCreated : IEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string Name { get; }

        public GameCreated(Guid id, string name) 
        {
            Id = id;
            Name = name;
        }
    }
}
