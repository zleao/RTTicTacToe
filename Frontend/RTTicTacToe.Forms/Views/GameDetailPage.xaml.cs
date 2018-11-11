using System;
using RTTicTacToe.Forms.Models;
using RTTicTacToe.Forms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RTTicTacToe.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GameDetailPage : ContentPage
    {
        readonly GameDetailViewModel _viewModel;

        public GameDetailPage()
            : this(null)
        {

        }
        public GameDetailPage(GameDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = _viewModel = viewModel;
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            // Manually deselect item.
            GameMovementsListView.SelectedItem = null;
        }

        private void MovementButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var row = Grid.GetRow(button);
            var col = Grid.GetColumn(button);

            _viewModel.MakeMovementCommand.Execute(new Coordinates(row, col));
        }
    }
}