using System;
using CQRSlite.Commands;

namespace RTTicTacToe.CQRS.WriteModel.Commands
{
    public class CreateGame : ICommand
    {
        public Guid Id { get; set; }
        public int ExpectedVersion { get; set; }
        public string Name { get; }

        public CreateGame(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
