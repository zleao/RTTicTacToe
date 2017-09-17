using NUnit.Framework;
using RTTicTacToe.Aggregates;
using RTTicTacToe.Commands;
using RTTicTacToe.Events;
using System;

namespace RTTicTacToe.CQRS.Tests
{
    [TestFixture]
    public class CheckersTests : BDDTest<GameAggregate>
    {
        private Guid game1Id;
        private string game1Name;
        private Guid player1Id;
        private string player1Name;

        [SetUp]
        public void Setup()
        {
            game1Id = Guid.NewGuid();
            game1Name = "Game 1";
            player1Id = Guid.NewGuid();
            player1Name = "Player 1";
        }

        [Test]
        public void CanCreateANewGame()
        {
            Test(
                Given(),
                When(new CreateGame
                {
                    Id = game1Id,
                    Name = game1Name
                }),
                Then(new GameCreated
                {
                    GameId = game1Id,
                    GameName = game1Name
                }));
        }

        [Test]
        public void CanRegisterANewPlayer()
        {
            Test(
                Given(),
                When(new RegisterPlayer
                {
                    Id = player1Id,
                    Name = player1Name,
                    GameId = game1Id
                }),
                Then(new PlayerRegistered
                {
                    PlayerId = player1Id,
                    PlayerName = player1Name,
                    GameId = game1Id
                }));
        }
    }
}
