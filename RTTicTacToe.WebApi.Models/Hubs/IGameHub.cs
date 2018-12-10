using System;
using System.Threading.Tasks;

namespace RTTicTacToe.WebApi.Models.Hubs
{
    public interface IGameHub
    {
        Task GameCreated();
        Task PlayerJoinedGame(Guid gameId);
        Task MovementMade(Guid gameId);
        Task GameFinished(Guid gameId);
    }
}
