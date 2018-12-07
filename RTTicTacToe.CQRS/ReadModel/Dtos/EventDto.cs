using System;
using CQRSlite.Events;

namespace RTTicTacToe.CQRS.ReadModel.Dtos
{
    public class EventDto : IEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string SerializedEvent { get; set; }
        public string EventType { get; set; }
    }
}
