using System;

namespace RTTicTacToe.CQRS.ReadModel.Dtos
{
    public class MovementDto
    {
        public Guid Id { get; }
        public int X { get; }
        public int Y { get; }
        public Guid PlayerId { get; }

        public MovementDto(Guid id, Guid playerId, int x, int y)
        {
            Id = id;
            PlayerId = playerId;
            X = x;
            Y = y;
        }
    }
}
