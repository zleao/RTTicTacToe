using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using RTTicTacToe.WebApi.Models.Hubs;

namespace RTTicTacToe.CQRS.Hubs
{
    /// <summary>
    /// SignalR hub for game.
    /// </summary>
    public class GameHub : Hub<IGameHub>
    {
        /// <summary>
        /// When a player whant to register himself to receive notifications of a game.
        /// </summary>
        /// <returns>The game.</returns>
        /// <param name="gameId">Game identifier.</param>
        public async Task RegisterForGame(Guid gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
        }

        /// <summary>
        /// When a player wants to stop receiving notifications from a game
        /// </summary>
        /// <returns>The game.</returns>
        /// <param name="gameId">Game identifier.</param>
        public async Task UnregisterForGame(Guid gameId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId.ToString());
        }
    }
}
