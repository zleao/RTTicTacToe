using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RTTicTacToe.Forms.Models;

namespace RTTicTacToe.Forms.ViewModels
{
    public class GameDetailViewModel : BaseViewModel
    {
        #region Properties

        private Guid _gameId;
        public Guid GameId
        {
            get => _gameId;
            set => SetProperty(ref _gameId, value);
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

        private ObservableCollection<Movement> _movements;
        public ObservableCollection<Movement> Movements
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

        #endregion

        #region Constructor

        public GameDetailViewModel(Game game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            RefreshGameValues(game);
        }

        #endregion

        #region Properties

        private void RefreshGameValues(Game game)
        {
            GameId = game.Id;
            Title = game.Name;
            Player1 = game.Player1;
            Player2 = game.Player2;
            Movements = new ObservableCollection<Movement>(game.Movements ?? new List<Movement>());
            Winner = game.Winner;
            Version = game.Version;
        }
      
        #endregion
    }
}
