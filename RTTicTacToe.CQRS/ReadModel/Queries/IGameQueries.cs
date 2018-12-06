using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RTTicTacToe.CQRS.ReadModel.Dtos;

namespace RTTicTacToe.CQRS.ReadModel.Queries
{
    public interface IGameQueries
    {
        Task<IList<GameDto>> GetAllGamesAsync();
        Task<GameDto> GetGameByIdAsync(Guid id);
        Task<IList<GameDto>> GetPlayerGamesAsync(Guid playerId);
        Task<IList<MovementDto>> GetGameMovementsAsync(Guid gameId);
    }
}