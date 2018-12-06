using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RTTicTacToe.CQRS.Database.Models
{
    public class Game
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastChangeDate { get; set; }
        public int Version { get; set; }

        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public Player Winner { get; set; }
        public ICollection<Movement> Movements { get; set; }
    }
}
