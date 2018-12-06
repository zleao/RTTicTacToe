using System;
using System.ComponentModel.DataAnnotations;

namespace RTTicTacToe.CQRS.Database.Models
{
    public class Movement
    {
        [Key]
        public Guid Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Guid PlayerId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastChangeDate { get; set; }

        public Game Game { get; set; }
    }
}
