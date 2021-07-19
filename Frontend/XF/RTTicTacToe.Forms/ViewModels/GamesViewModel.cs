using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CommonServiceLocator;
using RTTicTacToe.Forms.Messages;
using RTTicTacToe.Forms.Models;
using RTTicTacToe.Forms.Services;
using Xamarin.Forms;

namespace RTTicTacToe.Forms.ViewModels
{
    public class GamesViewModel : BaseViewModel
    {
        #region Fields

        private bool _isInitialized;

        #endregion

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

        private bool _isLoadingGames;
        public bool IsLoadingGames
        {
            get => _isLoadingGames;
            set => SetProperty(ref _isLoadingGames, value);
        }

        #endregion

        #region Commands

        public Command EnsureInitCommand { get; set; }
        public Command LoadGamesCommand { get; set; }
        public Command CreateNewGameCommand { get; set; }

        #endregion

        #region Constructor

        public GamesViewModel(IMessagingCenter messagingCenter,
                              IGameService gameService,
                              IGameHubService gameHubService,
                              ILocalStorageService localStorageService)
            : base(messagingCenter, gameService, gameHubService, localStorageService)
        {
            Title = "Games";
            Games = new ObservableCollection<GameDetailViewModel>();

            EnsureInitCommand = new Command(async () => await OnEnsureInitAsync());
            LoadGamesCommand = new Command(async () => await OnLoadGamesAsync());
            CreateNewGameCommand = new Command(async () => await OnCreateNewGameAsync());

            MessagingCenter.Subscribe<GameHubService, GameCreatedMessage>(this, nameof(GameCreatedMessage), HandleGameCreated);
        }

        #endregion

        #region Methods

        public async Task OnEnsureInitAsync()
        {
            try
            {
                //ensure the player is created
                var playerCreated = await EnsurePlayerIsCreatedAsync();
                while (!playerCreated) { playerCreated = await EnsurePlayerIsCreatedAsync(); };


                //ensure the game hub is started
                if(!GameHubService.IsOnline)
                {
                    IsBusy = true;

                    await GameHubService.StartHubAsync();
                }

                if(!_isInitialized)
                {
                    IsBusy = true;
                    await OnLoadGamesAsync();
                    _isInitialized = true;
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
            if (IsLoadingGames)
                return;

            IsLoadingGames = true;

            try
            {
                var games = await GameService.GetGamesAsync(true);

                Games = new ObservableCollection<GameDetailViewModel>();

                if (games?.Count() > 0)
                {
                    foreach (var game in games)
                    {
                        var newGame = ServiceLocator.Current.GetInstance<GameDetailViewModel>();
                        await newGame.InitializeValuesAsync(game, CurrentPlayer);

                        Games.Add(newGame);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsLoadingGames = false;
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

        private void HandleGameCreated(GameHubService sender, GameCreatedMessage message)
        {
            OnLoadGamesAsync();
        }

        #endregion
    }
}