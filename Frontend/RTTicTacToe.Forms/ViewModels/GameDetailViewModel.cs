﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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

        private int[,] _board;
        public int[,] Board
        {
            get => _board;
            set => SetProperty(ref _board, value);
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

        private ObservableCollection<EventExtended> _events;
        public ObservableCollection<EventExtended> Events
        {
            get => _events;
            set => SetProperty(ref _events, value);
        }

        #endregion

        #region Commands

        public Command JoinGameCommand { get; }
        public Command MakeMovementCommand { get; }
        public Command RefreshEventsCommand { get; }

        #endregion

        #region Constructor

        public GameDetailViewModel(Game currentGame, Player currentPlayer)
        {
            _currentGame = currentGame ?? throw new ArgumentNullException(nameof(currentGame));
            _currentPlayer = currentPlayer ?? throw new ArgumentNullException(nameof(currentPlayer));

            JoinGameCommand = new Command(async () => await OnJoinGameAsync(), CanJoinGame);
            MakeMovementCommand = new Command<Coordinates>(async (c) => await OnMakeMovementAsync(c));
            RefreshEventsCommand = new Command(async () => await OnRefreshEventsAsync());

            RefreshGameValuesAsync();
        }

        #endregion

        #region Methods

        private Task RefreshGameValuesAsync()
        {
            _gameGuid = _currentGame.Id;
            GameId = _gameGuid.ToString();
            GameName = _currentGame.Name;
            Title = _currentGame.Name;
            Player1 = _currentGame.Player1;
            Player2 = _currentGame.Player2;
            Board = _currentGame.Board;
            Winner = _currentGame.Winner;
            Version = _currentGame.Version;

            return OnRefreshEventsAsync();
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
                    await RefreshGameValuesAsync();
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

        private Task OnMakeMovementAsync(Coordinates coord)
        {
            return Task.CompletedTask;
        }

        private async Task OnRefreshEventsAsync()
        {
            try
            {
                IsBusy = true;

                var events = (await GameService.GetGameEventsAsync(_currentGame.Id)).Select(e => new EventExtended(e));
                Events = new ObservableCollection<EventExtended>(events);
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

        #endregion
    }
}