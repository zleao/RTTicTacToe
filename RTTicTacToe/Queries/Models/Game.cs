using System;
using System.Collections.Generic;

namespace RTTicTacToe.Queries.Models
{
    public class Game
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public List<Movement> Movements { get; set; }
        public Player Winner { get; set; }

        public Game(Guid id, string name)
        {
            Id = id;
            Name = name;
            Movements = new List<Movement>();
        }
    }
}
