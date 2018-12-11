using CommonServiceLocator;
using RTTicTacToe.Forms.Messages;
using RTTicTacToe.Forms.Services;
using Xamarin.Forms;

namespace RTTicTacToe.Forms.Views
{
    public abstract class BaseContentPage : ContentPage
    {
        #region Properties

        protected IMessagingCenter MessagingCenter => ServiceLocator.Current.GetInstance<IMessagingCenter>();

        protected IGameHubService GameHubService => ServiceLocator.Current.GetInstance<IGameHubService>();

        protected abstract ToolbarItem ConnStateToolbarItem { get; }

        #endregion

        #region Constructor

        protected BaseContentPage()
        {
            MessagingCenter.Subscribe<GameHubService, ConnectionStateChangedMessage>(this, nameof(ConnectionStateChangedMessage), HandleConnectionStateChanged);
        }

        #endregion

        #region Lifecycle

        protected override void OnAppearing()
        {
            UpdateConnectionStateToolbarItem();

            base.OnAppearing();
        }

        #endregion

        #region Methods

        protected virtual void HandleConnectionStateChanged(IGameHubService sender, ConnectionStateChangedMessage message)
        {
            UpdateConnectionStateToolbarItem();
        }

        protected virtual void UpdateConnectionStateToolbarItem()
        {
            Device.BeginInvokeOnMainThread(() => ConnStateToolbarItem.Icon = GameHubService.IsOnline ? "icon_online" : "icon_offline");
        }

        #endregion
    }
}
