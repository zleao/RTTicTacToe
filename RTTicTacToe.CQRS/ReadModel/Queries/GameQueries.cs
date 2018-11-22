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

        public Task<IList<GameDto>> GetGamesOfPlayerAsync(Guid playerId)
        {
            return _databaseService.GetGamesByPlayerIdAsync(playerId);
        }

        public async Task<IList<MovementDto>> GetMovementsFromGameAsync(Guid gameId)
        {
            return (await _databaseService.GetGameByIdAsync(gameId)).Movements;
        }

        public async Task<IList<PlayerDto>> GetPlayersFromGameAsync(Guid gameId)
        {
            var game = await _databaseService.GetGameByIdAsync(gameId);
            lock (game)
            {
                return new List<PlayerDto> { game.Player1, game.Player2 };
            }
        }

        #endregion
    }
}
