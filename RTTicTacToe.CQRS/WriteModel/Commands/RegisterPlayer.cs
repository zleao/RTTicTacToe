using System;
using CQRSlite.Commands;

namespace RTTicTacToe.CQRS.WriteModel.Commands
{
    public class RegisterPlayer : ICommand
    {
        public Guid Id { get; set; }
        public int ExpectedVersion { get; }
        public Guid PlayerId { get; }
        public string PlayerName { get; }

        public RegisterPlayer(Guid id, Guid playerId, string playerName, int originalVersion)
        {
            Id = id;
            PlayerId = playerId;
            PlayerName = playerName;
            ExpectedVersion = originalVersion;
        }
    }
}
