using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Commands;
using CQRSlite.Domain;
using RTTicTacToe.CQRS.WriteModel.Commands;
using RTTicTacToe.CQRS.WriteModel.Domain;

namespace RTTicTacToe.CQRS.WriteModel.Handlers
{
    public class GameCommandHandlers : ICommandHandler<CreateGame>,
        ICancellableCommandHandler<MakeMovement>,
        ICancellableCommandHandler<RegisterPlayer>
    {
        private readonly ISession _session;

        public GameCommandHandlers(ISession session)
        {
            _session = session;
        }

        public async Task Handle(CreateGame message)
        {
            var item = new GameAggregate(message.Id, message.Name);
            await _session.Add(item);
            await _session.Commit();
        }

        public async Task Handle(MakeMovement message, CancellationToken token = new CancellationToken())
        {
            var item = await _session.Get<GameAggregate>(message.Id, message.ExpectedVersion, token);
            item.MakeMovement(message.PlayerId, message.X, message.Y);
            await _session.Commit(token);
        }

        public async Task Handle(RegisterPlayer message, CancellationToken token = new CancellationToken())
        {
            var item = await _session.Get<GameAggregate>(message.Id, message.ExpectedVersion, token);
            item.RegisterPlayer(message.PlayerId, message.PlayerName);
            await _session.Commit(token);
        }
    }
}
