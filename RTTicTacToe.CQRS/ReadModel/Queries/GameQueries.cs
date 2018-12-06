using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RTTicTacToe.CQRS.ReadModel.Dtos;
using RTTicTacToe.CQRS.ReadModel.Infrastructure;

namespace RTTicTacToe.CQRS.ReadModel.Queries
{
    public class GameQueries : IGameQueries
    {
        #region Fields

        private readonly IDatabaseService _databaseService;

        #endregion

        #region Constructor

        public GameQueries(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        #endregion

        #region Queries

        public Task<IList<GameDto>> GetAllGamesAsync()
        {
            return _databaseService.GetAllGamesAsync();
        }
     
        public Task<GameDto> GetGameByIdAsync(Guid id)
        {
            return _databaseService.GetGameByIdAsync(id);
        }

        public Task<IList<GameDto>> GetPlayerGamesAsync(Guid playerId)
        {
            return _databaseService.GetPlayerGamesAsync(playerId);
        }

        public async Task<IList<MovementDto>> GetGameMovementsAsync(Guid gameId)
        {
            return (await _databaseService.GetGameByIdAsync(gameId)).Movements;
        }

        #endregion
    }
}
