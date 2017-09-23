using System;

namespace RTTicTacToe.Queries.Models
{
    public class Movement
    {
        public Guid Id { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Guid PlayerId { get; set; }

        public Movement(Guid id)
        {
            Id = id;
        }
    }
}
