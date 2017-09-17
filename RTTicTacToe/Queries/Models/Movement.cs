using System;

namespace RTTicTacToe.Queries.Models
{
    public class Movement
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Guid PlayerId { get; set; }
    }
}
