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
        #region Fields

        readonly GameDetailViewModel _viewModel;

        #endregion

        #region Constructor

        public GameDetailPage()
            : this(null)
        {

        }

        public GameDetailPage(GameDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = _viewModel = viewModel;
        }

        #endregion

        #region Lifecycle

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.Movements.CollectionChanged += Movements_CollectionChanged;
        }

        protected override void OnDisappearing()
        {
            _viewModel.Movements.CollectionChanged -= Movements_CollectionChanged;

            base.OnDisappearing();
        }

        #endregion

        #region Event Handlers

        private void SegControl_ValueChanged(object sender, SegmentedControl.FormsPlugin.Abstractions.ValueChangedEventArgs e)
        {
            GameMovementsListView.IsVisible = (e.NewValue == 0);
            GameEventsListView.IsVisible = (e.NewValue == 1); ;

            if(GameEventsListView.IsVisible && _viewModel.Events?.Count == 0)
            {
                _viewModel.RefreshEventsCommand.Execute(null);
            }
        }

        private void MovementButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var row = Grid.GetRow(button);
            var col = Grid.GetColumn(button);

            _viewModel.MakeMovementCommand.Execute(new Coordinates(row, col));
        }

        private void Movements_OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            // Manually deselect item.
            GameMovementsListView.SelectedItem = null;
        }

        private void Events_OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            // Manually deselect item.
            GameEventsListView.SelectedItem = null;
        }

        private void Movements_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                return;
            }

            foreach (MovementExtended item in e.NewItems)
            {
                UpdateSquare(item);
            }
        }

        #endregion

        #region Methods

        private void UpdateSquare(MovementExtended item)
        {
            Button btnToUpdate = null;
            if(item.X == 0)
            {
                if(item.Y == 0)
                {
                    btnToUpdate = MovBtn00;
                }
                else if(item.Y == 1)
                {
                    btnToUpdate = MovBtn01;
                }
                else if (item.Y == 2)
                {
                    btnToUpdate = MovBtn02;
                }
            }
            else if(item.X == 1)
            {
                if (item.Y == 0)
                {
                    btnToUpdate = MovBtn10;
                }
                else if (item.Y == 1)
                {
                    btnToUpdate = MovBtn11;
                }
                else if (item.Y == 2)
                {
                    btnToUpdate = MovBtn12;
                }
            }
            else if(item.X == 2)
            {
                if (item.Y == 0)
                {
                    btnToUpdate = MovBtn20;
                }
                else if (item.Y == 1)
                {
                    btnToUpdate = MovBtn21;
                }
                else if (item.Y == 2)
                {
                    btnToUpdate = MovBtn22;
                }
            }

            if(btnToUpdate != null)
            {
                btnToUpdate.Text = item.PlayerId == _viewModel.Player1.Id ? "X" : "O";
                btnToUpdate.IsEnabled = false;
            }
        }

        #endregion
    }
}