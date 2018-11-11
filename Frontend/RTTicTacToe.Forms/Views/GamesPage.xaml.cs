
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
            if (!(args.SelectedItem is GameDetailViewModel gameVM))
            {
                return;
            }

            await Navigation.PushAsync(new GameDetailPage(gameVM));

            // Manually deselect item.

            GamesListView.SelectedItem = null;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if(await viewModel.EnsurePlayerIsCreatedAsync())
            {
                if (viewModel.Games.Count == 0)
                {
                    viewModel.LoadGamesCommand.Execute(null);
                }
            }
        }
    }
}