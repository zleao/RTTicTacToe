using System;

namespace RTTicTacToe.Queries.Models
{
    public class Player
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }

        public Player(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
