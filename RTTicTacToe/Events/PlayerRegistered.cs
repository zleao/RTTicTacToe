using System;

namespace RTTicTacToe.Events
{
    public class PlayerRegistered
    {
        public Guid Id;
        public Guid PlayerId;
        public string PlayerName;
        public int PlayerNumber;
    }
}
