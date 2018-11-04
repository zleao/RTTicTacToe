using System;
using System.Collections.Generic;

namespace RTTicTacToe.Forms.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public List<Movement> Movements { get; set; }
        public Player Winner { get; set; }
        public int Version { get; set; }
    }
}