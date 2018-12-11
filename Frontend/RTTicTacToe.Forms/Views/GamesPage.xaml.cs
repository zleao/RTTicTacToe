using System;
using Acr.UserDialogs;
using CommonServiceLocator;
using RTTicTacToe.Forms.Services;
using RTTicTacToe.Forms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RTTicTacToe.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamesPage : BaseContentPage
    {
        #region Fields

        private readonly GamesViewModel _viewModel;

        #endregion

        #region Properties

        protected override ToolbarItem ConnStateToolbarItem => ConnectionStateToolbarItem;

        #endregion

        #region Constructor

        public GamesPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = ServiceLocator.Current.GetInstance<GamesViewModel>();
        }

        #endregion

        #region Lifecycle

        protected override void OnAppearing()
        {
            _viewModel.PropertyChanged += ViewModel_PropertyChanged;

            _viewModel.EnsureInitCommand.Execute(null);

            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            _viewModel.PropertyChanged -= ViewModel_PropertyChanged;

            base.OnDisappearing();
        }

        #endregion

        #region Event Handlers

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            // Manually deselect item.
            GamesListView.SelectedItem = null;

            if (!(args.SelectedItem is GameDetailViewModel gameVM))
            {
                return;
            }

            await Navigation.PushAsync(new GameDetailPage(gameVM));
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(_viewModel.IsBusy))
            {
                if(_viewModel.IsBusy)
                {
                    UserDialogs.Instance.ShowLoading();
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                }
            }
        }

        #endregion
    }
}