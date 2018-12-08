using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RTTicTacToe.CQRS.ReadModel.Dtos;

namespace RTTicTacToe.CQRS.ReadModel.Infrastructure
{
    public interface IDatabaseService
    {
        Task AddNewGameAsync(Guid gameId, string gameName, int gameVersion);
        Task UpdatePlayer1Async(Guid gameId, int gameVersion, PlayerDto player);
        Task UpdatePlayer2Async(Guid gameId, int gameVersion, PlayerDto player);
        Task UpdateWinnerAsync(Guid gameId, int gameVersion, Guid winnerId);
        Task UpdateGameBoardAsync(Guid gameId, int gameVersion, int playerNumber, int x, int y);

        Task<IList<GameDto>> GetAllGamesAsync();
        Task<GameDto> GetGameByIdAsync(Guid id);
        Task<int[,]> GetGameBoardAsync(Guid id);
        Task<IList<GameDto>> GetPlayerGamesAsync(Guid playerId);
    }
}
