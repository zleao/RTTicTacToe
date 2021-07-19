using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using RTTicTacToe.Forms.Messages;
using RTTicTacToe.Forms.Services;
using Xamarin.Forms;

namespace RTTicTacToe.Forms.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Properties

        public IMessagingCenter MessagingCenter { get; }
        public IGameService GameService { get; }
        public IGameHubService GameHubService { get; }
        public ILocalStorageService LocalStorageService { get; }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        #endregion

        #region Commands

        public Command TryReconnectCommand { get; }

        #endregion

        #region Constructor

        public BaseViewModel(IMessagingCenter messagingCenter, 
                             IGameService gameService, 
                             IGameHubService gameHubService,
                             ILocalStorageService localStorageService)
        {
            MessagingCenter = messagingCenter;
            GameService = gameService;
            GameHubService = gameHubService;
            LocalStorageService = localStorageService;

            TryReconnectCommand = new Command(async () => await OnTryReconnectAsync(), CanTryReconnect);

            messagingCenter.Subscribe<GameHubService, ConnectionStateChangedMessage>(this, nameof(ConnectionStateChangedMessage), HandleConnectionStateChanged);
        }

        #endregion

        #region Methods

        private bool CanTryReconnect()
        {
            return !GameHubService.IsOnline && !GameHubService.IsReconnecting;
        }
        private Task OnTryReconnectAsync()
        {
            return GameHubService.StartHubAsync();
        }

        protected virtual void HandleConnectionStateChanged(IGameHubService sender, ConnectionStateChangedMessage message)
        {
            Device.BeginInvokeOnMainThread(TryReconnectCommand.ChangeCanExecute);
        }

        #endregion

        #region INotifyPropertyChanged

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
