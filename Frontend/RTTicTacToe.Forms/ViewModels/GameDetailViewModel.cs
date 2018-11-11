using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using RTTicTacToe.Forms.Models;
using Xamarin.Forms;

namespace RTTicTacToe.Forms.ViewModels
{
    public class GameDetailViewModel : BaseViewModel
    {
        #region Fields

        private readonly Player _currentPlayer;
        private Game _currentGame;

        #endregion

        #region Properties

        private Guid _gameGuid;
        private string _gameId;
        public string GameId
        {
            get => _gameId;
            set => SetProperty(ref _gameId, value);
        }

        private string _gameName;
        public string GameName
        {
            get => _gameName;
            set => SetProperty(ref _gameName, value);
        }

        private Player _player1;
        public Player Player1
        {
            get => _player1;
            set => SetProperty(ref _player1, value);
        }

        private Player _player2;
        public Player Player2
        {
            get => _player2;
            set => SetProperty(ref _player2, value);
        }

        private ObservableCollection<MovementExtended> _movements;
        public ObservableCollection<MovementExtended> Movements
        {
            get => _movements;
            set => SetProperty(ref _movements, value);
        }

        private Player _winner;
        public Player Winner
        {
            get => _winner;
            set => SetProperty(ref _winner, value);
        }

        private int _version;
        public int Version
        {
            get => _version;
            set => SetProperty(ref _version, value);
        }

        private bool _canMakeMoves;
        public bool CanMakeMoves
        {
            get => _canMakeMoves;
            set => SetProperty(ref _canMakeMoves, value);
        }
        #endregion

        #region Commands

        public Command MakeMovementCommand { get; }
        public Command JoinGameCommand { get; }

        #endregion

        #region Constructor

        public GameDetailViewModel(Game currentGame, Player currentPlayer)
        {
            _currentGame = currentGame ?? throw new ArgumentNullException(nameof(currentGame));
            _currentPlayer = currentPlayer ?? throw new ArgumentNullException(nameof(currentPlayer));

            MakeMovementCommand = new Command<Coordinates>(OnMakeMovement);
            JoinGameCommand = new Command(async() => await OnJoinGameAsync(), CanJoinGame);

            RefreshGameValues();
        }

        #endregion

        #region Methods

        private void RefreshGameValues()
        {
            _gameGuid = _currentGame.Id;
            GameId = _gameGuid.ToString();
            GameName = _currentGame.Name;
            Title = _currentGame.Name;
            Player1 = _currentGame.Player1;
            Player2 = _currentGame.Player2;
            Movements = new ObservableCollection<MovementExtended>(_currentGame.Movements.Select(m => new MovementExtended(m, Player1, Player2)) ?? new List<MovementExtended>());
            Winner = _currentGame.Winner;
            Version = _currentGame.Version;
        }

        private bool CanJoinGame()
        {
            var alreadyJoined = (Player1?.Id == _currentPlayer.Id || Player2?.Id == _currentPlayer.Id);
            return !alreadyJoined && (Player1 == null || Player2 == null);
        }

        private async Task OnJoinGameAsync()
        {
            try
            {
                IsBusy = true;

                await GameService.AddPlayerAsync(_gameGuid, Version, _currentPlayer.Id, _currentPlayer.Name);
                var updatedGame = await GameService.GetGameAsync(_gameGuid);
                if (updatedGame != null)
                {
                    _currentGame = updatedGame;
                    RefreshGameValues();
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Error", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnMakeMovement(Coordinates coord)
        {
        }
     
        #endregion
    }
}
