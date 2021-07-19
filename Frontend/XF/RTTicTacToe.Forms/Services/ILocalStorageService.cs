using RTTicTacToe.Forms.Models;

namespace RTTicTacToe.Forms.Services
{
    public interface ILocalStorageService
    {
        Player GetStoredPlayer();
        bool SavePlayer(Player player);
    }
}
