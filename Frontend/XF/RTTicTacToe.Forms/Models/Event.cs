using System;
namespace RTTicTacToe.Forms.Models
{
    public class Event
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string SerializedEvent { get; set; }
        public string EventType { get; set; }
    }
}
