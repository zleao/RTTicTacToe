using CommonServiceLocator;
using RTTicTacToe.Forms.Services;
using RTTicTacToe.Forms.ViewModels;
using Unity;
using Unity.ServiceLocation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RTTicTacToe.Forms
{
    public partial class App : Application
    {
        public static string AzureBackendUrl = "https://rttictactoe.azurewebsites.net";         // Dev env
        //public static string AzureBackendUrl = "https://localhost:44397";                     // IIS with Swagger
        //public static string AzureBackendUrl = "http://localhost:5000";                       // Console
        //public static string AzureBackendUrl = "http://10.0.2.2:5000";                        // Android emulator => localhost:5000
        //public static string AzureBackendUrl = "http://10.0.2.2:44397";                       // Android emulator => localhost:44397

        public App()
        {
            InitializeComponent();

            RegisterDependencies();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
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
