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
        public ObservableCollection<Game> Games { get; set; }
        public Command LoadGamesCommand { get; set; }
        public Command CreateNewGameCommand { get; set; }

        public GamesViewModel()
        {
            Title = "Games";
            Games = new ObservableCollection<Game>();
            LoadGamesCommand = new Command(async () => await ExecuteLoadGamesCommand());
            CreateNewGameCommand = new Command(async () => await ExecuteCreateNewGameCommand());
        }

        async Task ExecuteCreateNewGameCommand()
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

        async Task ExecuteLoadGamesCommand()
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
                    Games.Add(game);
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
    }
}