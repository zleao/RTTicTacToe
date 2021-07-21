using System;
using System.Windows.Input;
using RTTicTacToe.Forms.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RTTicTacToe.Forms.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel(IMessagingCenter messagingCenter,
                              IGameService gameService,
                              IGameHubService gameHubService,
                              ILocalStorageService localStorageService)
            : base(messagingCenter, gameService, gameHubService, localStorageService)
        {
            Title = "About";

            OpenWebCommand = new Command(async () => await Launcher.OpenAsync(new Uri("https://github.com/zleao/RTTicTacToe")));
        }

        public ICommand OpenWebCommand { get; }
    }
}