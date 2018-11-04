
using RTTicTacToe.Forms.Models;
using RTTicTacToe.Forms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RTTicTacToe.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamesPage : ContentPage
    {
        GamesViewModel viewModel;

        public GamesPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new GamesViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (!(args.SelectedItem is Game game))
            {
                return;
            }

            await Navigation.PushAsync(new GameDetailPage(new GameDetailViewModel(game)));

            // Manually deselect item.

            GamesListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Games.Count == 0)
                viewModel.LoadGamesCommand.Execute(null);
        }
    }
}