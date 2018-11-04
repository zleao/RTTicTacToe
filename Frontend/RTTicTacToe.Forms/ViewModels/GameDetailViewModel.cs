using RTTicTacToe.Forms.Models;

namespace RTTicTacToe.Forms.ViewModels
{
    public class GameDetailViewModel : BaseViewModel
    {
        public Game Game { get; set; }
        public GameDetailViewModel(Game game = null)
        {
            Title = game?.Name;
            Game = game;
        }
    }
}
