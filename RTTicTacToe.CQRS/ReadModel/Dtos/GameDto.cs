using System;
using System.Collections.Generic;

namespace RTTicTacToe.CQRS.ReadModel.Dtos
{
    public class GameDto
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public PlayerDto Player1 { get; set; }
        public PlayerDto Player2 { get; set; }
        public IList<MovementDto> Movements { get; set; }
        public PlayerDto Winner { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastChangeDate { get; set; }
        public int Version { get; set; }

        public GameDto(Guid id, string name, int version)
        {
            Id = id;
            Name = name;
            Version = version;
            Movements = new List<MovementDto>();
        }
    }
}
