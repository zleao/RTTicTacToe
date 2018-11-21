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
        Task<IList<GameDto>> GetGamesOfPlayerAsync(Guid playerId);
        Task<IList<PlayerDto>> GetPlayersFromGameAsync(Guid gameId);
        Task<IList<MovementDto>> GetMovementsFromGameAsync(Guid gameId);
    }
}