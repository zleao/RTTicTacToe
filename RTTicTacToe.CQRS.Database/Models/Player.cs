using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RTTicTacToe.CQRS.Database.Models
{
    public class Player
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastChangeDate { get; set; }

        [InverseProperty("Player1")]
        public ICollection<Game> GamesAsPlayer1 { get; set; }

        [InverseProperty("Player2")]
        public ICollection<Game> GamesAsPlayer2 { get; set; }

        [InverseProperty("Winner")]
        public ICollection<Game> GamesWon { get; set; }
    }
}
