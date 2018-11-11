using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RTTicTacToe.Forms.Models;

namespace RTTicTacToe.Forms.Services
{
    public interface IGameService
    {
        Task<IEnumerable<Game>> GetGamesAsync(bool onlyActive, bool forceRefresh = false);
        Task<bool> AddGameAsync(string name);
        Task<Game> GetGameAsync(Guid id);
        Task<bool> AddPlayerAsync(Guid gameId, int gameVersion, Guid playerId, string playerName);
        Task<bool> MakeMovement(Guid gameId, int gameVersion, Guid playerId, short x, short y);

        bool IsValidPlayer(Player player);
    }
}