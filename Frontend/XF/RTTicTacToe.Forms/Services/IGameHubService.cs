using System;
using System.Threading.Tasks;

namespace RTTicTacToe.Forms.Services
{
    public interface IGameHubService
    {
        bool IsReconnecting { get; }
        bool IsOnline { get; }
        Task StartHubAsync();
        Task RegisterForGame(Guid gameId);
        Task UnregisterForGame(Guid gameId);
    }
}
