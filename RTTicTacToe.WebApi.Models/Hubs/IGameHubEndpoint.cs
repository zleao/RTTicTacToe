using System;
using System.Threading.Tasks;

namespace RTTicTacToe.WebApi.Models.Hubs
{
    public interface IGameHubEndpoint
    {
        Task RegisterForGame(Guid gameId);
        Task UnregisterForGame(Guid gameId);
    }
}
