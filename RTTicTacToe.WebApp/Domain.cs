using RTTicTacToe.Aggregates;
using RTTicTacToe.CQRS;
using RTTicTacToe.CQRS.EventStore;
using RTTicTacToe.Queries;
using RTTicTacToe.Queries.Models;
using System;
using System.Collections.Generic;

namespace RTTicTacToe.WebApp
{
    public static class Domain
    {
        //public static readonly List<Game> Games = new List<Game>
        //{
        //    new Game(Guid.NewGuid(), "Game 1"),
        //    new Game(Guid.NewGuid(), "Game 2"),
        //    new Game(Guid.NewGuid(), "Game 3"),
        //};

        public static MessageDispatcher Dispatcher;
        public static IGameQueries GameQueries;

        public static void Setup()
        {
            Dispatcher = new MessageDispatcher(new InMemoryEventStore());

            Dispatcher.ScanInstance(new GameAggregate());

            GameQueries = new GameQueries();
            Dispatcher.ScanInstance(GameQueries);
        }
    }
}
