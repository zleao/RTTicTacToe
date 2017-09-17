using System;

namespace RTTicTacToe.Commands
{
    public class RegisterPlayer
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int PlayerNumber { get; set; }
    }
}
