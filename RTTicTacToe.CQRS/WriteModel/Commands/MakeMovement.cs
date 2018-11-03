using System;
using CQRSlite.Commands;

namespace RTTicTacToe.CQRS.WriteModel.Commands
{
    public class MakeMovement : ICommand
    {
        public Guid Id { get; set; }
        public int ExpectedVersion { get; set; }
        public Guid PlayerId { get; }
        public int X { get; }
        public int Y { get; }

        public MakeMovement(Guid id, Guid playerId, int x, int y, int originalVersion)
        {
            Id = id;
            PlayerId = playerId;
            X = x;
            Y = y;
            ExpectedVersion = originalVersion;
        }
    }
}
