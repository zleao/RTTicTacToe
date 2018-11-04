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

        public App()
        {
            InitializeComponent();

            DependencyService.Register<IGameService,GameService>();

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
    }
}
