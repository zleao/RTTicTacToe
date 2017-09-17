using System;
using System.Collections;
using System.Collections.Generic;

namespace RTTicTacToe.CQRS.EventStore
{
    public interface IEventStore
    {
        IEnumerable LoadEventsFor<TAggregate>(Guid id);
        void SaveEventsFor<TAggregate>(Guid id, int eventsLoaded, ArrayList newEvents);
    }
}
