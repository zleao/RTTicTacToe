using System;

namespace RTTicTacToe.Forms.Models
{
    public class Movement
    {
        public Guid Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Guid PlayerId { get; set; }
    }
}
