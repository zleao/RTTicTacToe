using System;
using System.Collections.Generic;
using RTTicTacToe.CQRS.ReadModel.Dtos;

namespace RTTicTacToe.CQRS.ReadModel.Infrastructure
{
    public static class InMemoryDatabase
    {
        public static readonly Dictionary<Guid, GameDto> AllGames = new Dictionary<Guid, GameDto>();
    }
}
