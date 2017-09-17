using System;

namespace RTTicTacToe.Events
{
    public class MovementMade
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
