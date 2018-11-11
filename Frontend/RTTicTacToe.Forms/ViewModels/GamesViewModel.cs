using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Acr.UserDialogs;
using RTTicTacToe.Forms.Models;
using Xamarin.Forms;

namespace RTTicTacToe.Forms.ViewModels
{
    public class GamesViewModel : BaseViewModel
    {
        #region Properties

        private Player _currentPlayer;
        public Player CurrentPlayer
        {
            get => _currentPlayer;
            set => SetProperty(ref _currentPlayer, value);
        }

        private ObservableCollection<GameDetailViewModel> _games;
        public ObservableCollection<GameDetailViewModel> Games
        {
            get => _games;
            set => SetProperty(ref _games, value);
        }

        #endregion

        #region Commands

        public Command LoadGamesCommand { get; set; }
        public Command CreateNewGameCommand { get; set; }

        #endregion

        #region Constructor

        public GamesViewModel()
        {
            Title = "Games";
            Games = new ObservableCollection<GameDetailViewModel>();
            LoadGamesCommand = new Command(async () => await OnLoadGamesAsync());
            CreateNewGameCommand = new Command(async () => await OnCreateNewGameAsync());
        }

        #endregion

        #region Methods

        private async Task OnCreateNewGameAsync()
        {
            var result = await UserDialogs.Instance.PromptAsync(new PromptConfig
            {
                Title = "Game Name",
                OnTextChanged = (args) => { args.IsValid = args.Value?.Length > 0; }
            });

            if (result != null && result.Ok)
            {
                await GameService.AddGameAsync(result.Value);
            }
        }

        private async Task OnLoadGamesAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Games.Clear();
                var games = await GameService.GetGamesAsync(false, true);
                foreach (var game in games)
                {
                    Games.Add(new GameDetailViewModel(game, CurrentPlayer));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task<bool> EnsurePlayerIsCreatedAsync()
        {
            var storedPlayer = LocalStorageService.GetStoredPlayer();
            if (GameService.IsValidPlayer(storedPlayer))
            {
                CurrentPlayer = storedPlayer;
                return true;
            }

            var result = await UserDialogs.Instance.PromptAsync(new PromptConfig
            {
                Title = "Player Name",
                OnTextChanged = (args) => { args.IsValid = args.Value?.Length > 0; }
            });

            if (result != null && result.Ok)
            {
                var newPlayer = new Player()
                {
                    Id = Guid.NewGuid(),
                    Name = result.Value
                };

                if(LocalStorageService.SavePlayer(newPlayer))
                {
                    CurrentPlayer = newPlayer;
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}