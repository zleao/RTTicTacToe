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
        Task<int[,]> GetGameBoardAsync(Guid gameId);
        Task<IList<EventDto>> GetGameEventsAsync(Guid gameId);
    }
}