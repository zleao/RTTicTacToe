using System;

namespace RTTicTacToe.Events
{
    public class GameCreated
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
