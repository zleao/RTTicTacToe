using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CQRSlite.Events;

namespace RTTicTacToe.CQRS.Database.Models
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventId { get; set; }

        public Guid Id { get; set; } //Id of the aggregate this event belongs to
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string SerializedEvent { get; set; }
        public string EventType { get; set; }
    }
}
