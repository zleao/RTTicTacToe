using System;

namespace RTTicTacToe.CQRS.ReadModel.Dtos
{
    public class PlayerDto
    {
        public Guid Id { get; }
        public string Name { get; set; }

        public PlayerDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
