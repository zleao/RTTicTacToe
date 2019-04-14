using System;
using System.Collections.Generic;
using System.Linq;
using CQRSlite.Events;
using RTTicTacToe.CQRS.ReadModel.Events;
using RTTicTacToe.CQRS.Tests.TestHelpers;
using RTTicTacToe.CQRS.WriteModel.Commands;
using RTTicTacToe.CQRS.WriteModel.Domain;
using RTTicTacToe.CQRS.WriteModel.Handlers;
using Xunit;

namespace RTTicTacToe.CQRS.Tests.WriteModel
{
    public class WhenGameCreated : Specification<GameAggregate, GameCommandHandlers, CreateGame>
    {
        private Guid _id;
        private readonly string _gameName = "NewGame";

        protected override IEnumerable<IEvent> Given()
        {
            _id = Guid.NewGuid();
            return new List<IEvent>();
        }

        protected override CreateGame When()
        {
            return new CreateGame(_id, _gameName);
        }

        protected override GameCommandHandlers BuildHandler()
        {
            return new GameCommandHandlers(Session);
        }

        [Then]
        public void Should_create_one_event()
        {
            Assert.Equal(1, PublishedEvents.Count);
        }

        [Then]
        public void Should_create_correct_event()
        {
            Assert.IsType<GameCreated>(PublishedEvents.First());
        }

        [Then]
        public void Should_save_name()
        {
            Assert.Equal(_gameName, ((GameCreated)PublishedEvents.First()).Name);
        }
    }
}
