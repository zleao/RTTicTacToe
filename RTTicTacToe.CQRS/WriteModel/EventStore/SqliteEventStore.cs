using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Events;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RTTicTacToe.CQRS.Database;
using RTTicTacToe.CQRS.Database.Models;

namespace RTTicTacToe.CQRS.WriteModel.EventStore
{
    public class SqliteEventStore : IEventStore
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IEventPublisher _publisher;

        public SqliteEventStore(DatabaseContext databaseContext, IEventPublisher publisher)
        {
            _databaseContext = databaseContext;
            _publisher = publisher;
        }

        public async Task<IEnumerable<IEvent>> Get(Guid aggregateId, int fromVersion, CancellationToken cancellationToken = default(CancellationToken))
        {
            var filteredList = await _databaseContext.Events.AsNoTracking().Where(e => e.Id == aggregateId && e.Version > fromVersion).ToListAsync();
            var selectedList = filteredList.Select(e => JsonConvert.DeserializeObject(e.SerializedEvent, Type.GetType(e.EventType)) as IEvent).ToList();

            return selectedList;
        }

        public async Task Save(IEnumerable<IEvent> events, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _databaseContext.Events.AddRangeAsync(events.Select(e => new Event
            {
                Id = e.Id,
                Version = e.Version,
                TimeStamp = e.TimeStamp,
                EventType = e.GetType().FullName,
                SerializedEvent = JsonConvert.SerializeObject(e)
            }));

            await _databaseContext.SaveChangesAsync(cancellationToken);

            foreach (var @event in events)
            {
                await _publisher.Publish(@event, cancellationToken);
            }
        }
    }
}
