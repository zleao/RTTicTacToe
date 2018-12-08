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
        private readonly IDatabaseService _databaseService;

        public GameView(IDatabaseService databaseService)
        {
            this._databaseService = databaseService;
        }

        public Task Handle(GameCreated message, CancellationToken token = new CancellationToken())
        {
            return _databaseService.AddNewGameAsync(message.Id, message.Name, message.Version);
        }

        public async Task Handle(PlayerRegistered message, CancellationToken token = new CancellationToken())
        {
            var newPlayer = new PlayerDto(message.PlayerId, message.PlayerName);

            if (message.PlayerNumber == 1)
            {
                await _databaseService.UpdatePlayer1Async(message.Id, message.Version, newPlayer);
            }
            else if (message.PlayerNumber == 2)
            {
                await _databaseService.UpdatePlayer2Async(message.Id, message.Version, newPlayer);
            }
        }

        public async Task Handle(MovementMade message, CancellationToken token = new CancellationToken())
        {
            await _databaseService.UpdateGameBoardAsync(message.Id,
                                                        message.Version,
                                                        message.PlayerNumber,
                                                        message.X, 
                                                        message.Y);


            var game = await _databaseService.GetGameByIdAsync(message.Id);
            if (GameHelper.CheckGameFinished(game))
            {
                await _databaseService.UpdateWinnerAsync(message.Id, message.Version, message.PlayerId);
            }
        }
    }
}
