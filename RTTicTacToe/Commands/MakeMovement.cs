using System;

namespace RTTicTacToe.Commands
{
    public class MakeMovement
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
