using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Events;
using RTTicTacToe.CQRS.ReadModel.Dtos;
using RTTicTacToe.CQRS.ReadModel.Events;
using RTTicTacToe.CQRS.ReadModel.Infrastructure;
using RTTicTacToe.CQRS.Utilities;

namespace RTTicTacToe.CQRS.ReadModel.Handlers
{
    public class GameView : ICancellableEventHandler<GameCreated>,
        ICancellableEventHandler<PlayerRegistered>,
        ICancellableEventHandler<MovementMade>
    {
        public Task Handle(GameCreated message, CancellationToken token = new CancellationToken())
        {
            InMemoryDatabase.AllGames.Add(message.Id, new GameDto(message.Id, message.Name, message.Version));
            return Task.CompletedTask;
            
        }

        public Task Handle(PlayerRegistered message, CancellationToken token = new CancellationToken())
        {
            var game = GameHelper.GetGame(message.Id);
            lock (game)
            {
                game.Version = message.Version;
                var newPlayer = new PlayerDto(message.PlayerId, message.PlayerName);

                if (message.PlayerNumber == 1)
                {
                    game.Player1 = newPlayer;
                }
                else if (message.PlayerNumber == 2)
                {
                    game.Player2 = newPlayer;
                }
            }

            return Task.CompletedTask;
        }

        public Task Handle(MovementMade message, CancellationToken token = new CancellationToken())
        {
            var game = GameHelper.GetGame(message.Id);
            lock (game)
            {
                game.Version = message.Version;
                game.Movements.Add(new MovementDto(message.MovementId, message.PlayerId, message.X, message.Y));

                if(GameHelper.CheckGameFinished(game))
                {
                    game.Winner = (game.Player1.Id == message.PlayerId ? game.Player1 : game.Player2);
                }
            }

            return  Task.CompletedTask;
        }
    }
}
