using RTTicTacToe.Forms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RTTicTacToe.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GameDetailPage : ContentPage
    {
        readonly GameDetailViewModel _viewModel;

        public GameDetailPage(GameDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = _viewModel = viewModel;
        }
    }
}