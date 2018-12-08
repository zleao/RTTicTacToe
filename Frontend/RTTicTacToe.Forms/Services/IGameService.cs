using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RTTicTacToe.Forms.Models;

namespace RTTicTacToe.Forms.Services
{
    public interface IGameService
    {
        Task<IEnumerable<Game>> GetGamesAsync(bool forceRefresh = false);
        Task<bool> AddGameAsync(string name);
        Task<Game> GetGameAsync(Guid id);
        Task<bool> AddPlayerAsync(Guid gameId, int gameVersion, Guid playerId, string playerName);
        Task<bool> MakeMovementAsync(Guid gameId, int gameVersion, Guid playerId, int x, int y);
        Task<int[,]> GetGameBoardAsync(Guid gameId);
        Task<IList<Event>> GetGameEventsAsync(Guid gameId);

        bool IsValidPlayer(Player player);
    }
}