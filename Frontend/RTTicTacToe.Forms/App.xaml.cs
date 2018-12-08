using System;
using RTTicTacToe.Forms.Services;
using RTTicTacToe.Forms.Views;
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

            RegisterClasses();

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

        private void RegisterClasses()
        {
            DependencyService.Register<IGameService, GameService>();
            DependencyService.Register<ILocalStorageService, LocalStorageService>();
        }
    }
}
