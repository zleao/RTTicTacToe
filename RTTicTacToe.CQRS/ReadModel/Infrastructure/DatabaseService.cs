using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RTTicTacToe.CQRS.ReadModel.Dtos;

namespace RTTicTacToe.CQRS.ReadModel.Infrastructure
{
    public class DatabaseService : IDatabaseService
    {
        public DatabaseService()
        {
        }

        public Task AddGameAsync(GameDto game)
        {
            throw new NotImplementedException();
        }

        public Task<IList<GameDto>> GetAllGamesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GameDto> GetGameByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<GameDto> GetGamesByPlayerIdAsync(Guid playerId)
        {
            throw new NotImplementedException();
        }
    }
}
