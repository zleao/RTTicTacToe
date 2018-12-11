using System;
using Acr.UserDialogs;
using RTTicTacToe.Forms.Models;
using RTTicTacToe.Forms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RTTicTacToe.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GameDetailPage : BaseContentPage
    {
        #region Fields

        readonly GameDetailViewModel _viewModel;

        #endregion

        #region Properties

        protected override ToolbarItem ConnStateToolbarItem => ConnectionStateToolbarItem;

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

            UpdateConnectionStateToolbarItem();

            UpdateBoardLayout();
        }

        #endregion

        #region Lifecycle

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        protected override void OnDisappearing()
        {
            _viewModel.PropertyChanged -= ViewModel_PropertyChanged;

            base.OnDisappearing();
        }

        #endregion

        #region Event Handlers

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.IsBusy))
            {
                if (_viewModel.IsBusy)
                {
                    UserDialogs.Instance.ShowLoading();
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                }
            }
            else if (e.PropertyName == nameof(_viewModel.Board))
            {
                UpdateBoardLayout();
            }
        }

        private void MovementButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var row = Grid.GetRow(button);
            var col = Grid.GetColumn(button);

            _viewModel.MakeMovementCommand.Execute(new Coordinates(row, col));
        }

        private void Events_OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            // Manually deselect item.
            GameEventsListView.SelectedItem = null;
        }

        #endregion

        #region Methods

        private void UpdateBoardLayout()
        {
            MovBtn00.Text = GetBoardBtnText(_viewModel.Board[0, 0]);
            MovBtn01.Text = GetBoardBtnText(_viewModel.Board[0, 1]);
            MovBtn02.Text = GetBoardBtnText(_viewModel.Board[0, 2]);
            MovBtn10.Text = GetBoardBtnText(_viewModel.Board[1, 0]);
            MovBtn11.Text = GetBoardBtnText(_viewModel.Board[1, 1]);
            MovBtn12.Text = GetBoardBtnText(_viewModel.Board[1, 2]);
            MovBtn20.Text = GetBoardBtnText(_viewModel.Board[2, 0]);
            MovBtn21.Text = GetBoardBtnText(_viewModel.Board[2, 1]);
            MovBtn22.Text = GetBoardBtnText(_viewModel.Board[2, 2]);
        }

        private string GetBoardBtnText(int value)
        {
            if(value == 1)
            {
                return "X";
            }

            if(value == 2)
            {
                return "O";
            }

            return string.Empty;
        }

        #endregion
    }
}