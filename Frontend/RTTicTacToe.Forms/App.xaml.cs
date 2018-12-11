using System;
using CommonServiceLocator;
using RTTicTacToe.Forms.Services;
using RTTicTacToe.Forms.ViewModels;
using RTTicTacToe.Forms.Views;
using Unity;
using Unity.ServiceLocation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace RTTicTacToe.Forms
{
    public partial class App : Application
    {
        public static string AzureBackendUrl = "https://rttictactoe.azurewebsites.net";
        //public static string AzureBackendUrl = "http://localhost:5000";

        public App()
        {
            InitializeComponent();

            RegisterDependencies();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private void RegisterDependencies()
        {
            var unityContainer = new UnityContainer();

            //Register services
            unityContainer.RegisterSingleton<IMessagingCenter, MessagingCenter>();
            unityContainer.RegisterSingleton<IGameHubService, GameHubService>();
            unityContainer.RegisterSingleton<IGameService, GameService>();
            unityContainer.RegisterSingleton<ILocalStorageService, LocalStorageService>();

            //Register viewmodels
            unityContainer.RegisterType<AboutViewModel>();
            unityContainer.RegisterType<GamesViewModel>();
            unityContainer.RegisterType<GameDetailViewModel>();

            //Set locator provider
            var unityServiceLocator = new UnityServiceLocator(unityContainer);
            ServiceLocator.SetLocatorProvider(() => unityServiceLocator);
        }
    }
}
