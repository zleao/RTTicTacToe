using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSlite.Events;
using Newtonsoft.Json;
using RTTicTacToe.CQRS.ReadModel.Dtos;
using RTTicTacToe.CQRS.ReadModel.Infrastructure;

namespace RTTicTacToe.CQRS.ReadModel.Queries
{
    public class GameQueries : IGameQueries
    {
        #region Fields

        private readonly IDatabaseService _databaseService;
        private readonly IEventStore _eventStore;

        #endregion

        #region Constructor

        public GameQueries(IDatabaseService databaseService, IEventStore eventStore)
        {
            _databaseService = databaseService;
            _eventStore = eventStore;
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

        public async Task<IList<EventDto>> GetGameEventsAsync(Guid gameId)
        {
            var events = await _eventStore.Get(gameId, 0);
            return events.Select(e => new EventDto
            {
                Id = e.Id,
                Version = e.Version,
                TimeStamp = e.TimeStamp,
                EventType = e.GetType().FullName,
                SerializedEvent = JsonConvert.SerializeObject(e)
            }).ToList();
        }

        #endregion
    }
}
