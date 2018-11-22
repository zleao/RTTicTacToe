using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RTTicTacToe.CQRS.ReadModel.Dtos;

namespace RTTicTacToe.CQRS.ReadModel.Infrastructure
{
    public interface IDatabaseService
    {
        Task<IList<GameDto>> GetAllGamesAsync();
        Task<GameDto> GetGameByIdAsync(Guid id);
        Task<IList<GameDto>> GetGamesByPlayerIdAsync(Guid playerId);
        Task AddGameAsync(GameDto game);
    }
}
