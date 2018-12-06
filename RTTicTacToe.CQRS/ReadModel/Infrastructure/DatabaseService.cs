using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RTTicTacToe.CQRS.Database;
using RTTicTacToe.CQRS.ReadModel.Dtos;

namespace RTTicTacToe.CQRS.ReadModel.Infrastructure
{
    public class DatabaseService : IDatabaseService
    {
        private readonly DatabaseContext _databaseContext;

        public DatabaseService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        #region Games

        public async Task AddNewGameAsync(Guid gameId, string gameName, int gameVersion)
        {
            var dateTimeNow = DateTime.Now;

            await _databaseContext.Games.AddAsync(new Database.Models.Game
            {
                Id = gameId,
                Name = gameName,
                Version = gameVersion,
                CreationDate = dateTimeNow,
                LastChangeDate = dateTimeNow
            });

            await _databaseContext.SaveChangesAsync();
        }

        public async Task<IList<GameDto>> GetAllGamesAsync()
        {
            return await _databaseContext.Games.AsNoTracking()
                                               .Include(g => g.Player1)
                                               .Include(g => g.Player2)
                                               .Include(g => g.Winner)
                                               .Include(g => g.Movements)
                                               .Select(g => g.ConvertToModelDto()).ToListAsync();
        }

        public async Task<GameDto> GetGameByIdAsync(Guid id)
        {
            var game = await _databaseContext.Games.AsNoTracking()
                                                   .Include(g => g.Player1)
                                                   .Include(g => g.Player2)
                                                   .Include(g => g.Winner)
                                                   .Include(g => g.Movements)
                                                   .FirstOrDefaultAsync(g =>g.Id == id);

            return game.ConvertToModelDto();
        }

        public async Task<IList<GameDto>> GetPlayerGamesAsync(Guid playerId)
        {
            var filteredGames = await _databaseContext.Games
                                                      .AsNoTracking()
                                                      .Include(g => g.Player1)
                                                      .Include(g => g.Player2)
                                                      .Include(g => g.Winner)
                                                      .Include(g => g.Movements)
                                                      .Where(g => (g.Player1 != null && g.Player1.Id == playerId) ||
                                                                  (g.Player2 != null && g.Player2.Id == playerId))
                                                      .ToListAsync();

            return filteredGames.Select(g => g.ConvertToModelDto()).ToList();
        }

        #endregion

        #region Players

        public Task UpdatePlayer1Async(Guid gameId, int gameVersion, PlayerDto player)
        {
            return UpdatePlayerAsync(gameId, gameVersion, player, 1);
        }

        public Task UpdatePlayer2Async(Guid gameId, int gameVersion, PlayerDto player)
        {
            return UpdatePlayerAsync(gameId, gameVersion, player, 2);
        }

        private async Task UpdatePlayerAsync(Guid gameId, int gameVersion, PlayerDto player, int playerNumber)
        {
            var dateTimeNow = DateTime.Now;

            var dbGame = await _databaseContext.Games.FirstAsync(g => g.Id == gameId);

            //Update or create the player
            var playerDb = await _databaseContext.Players.FirstOrDefaultAsync(p => p.Id == player.Id);
            if (playerDb == null)
            {
                playerDb = player.ConvertToModelDb();
                playerDb.CreationDate = dateTimeNow;
            }
            playerDb.LastChangeDate = dateTimeNow;

            dbGame.LastChangeDate = dateTimeNow;
            dbGame.Version = gameVersion;

            if(playerNumber == 1)
            {
                dbGame.Player1 = playerDb;
            }
            else if(playerNumber == 2)
            {
                dbGame.Player2 = playerDb;
            }

            await _databaseContext.SaveChangesAsync();
        }

        public async Task UpdateWinnerAsync(Guid gameId, int gameVersion, Guid winnerId)
        {
            var dbGame = await _databaseContext.Games.FirstAsync(g => g.Id == gameId);

            dbGame.LastChangeDate = DateTime.Now;
            dbGame.Version = gameVersion;
            dbGame.Winner = await _databaseContext.Players.FirstOrDefaultAsync(p => p.Id == winnerId);

            await _databaseContext.SaveChangesAsync();
        }

        #endregion

        #region Movements

        public async Task<IList<MovementDto>> GetGameMovementsAsync(Guid gameId)
        {
            var movements = await _databaseContext.Movements.Where(m => m.Game.Id == gameId).ToListAsync();

            return movements.Select(p => p.ConvertToModelDto()).ToList();
        }

        public async Task UpdateGameMovementsAsync(Guid gameId, int gameVersion, MovementDto movement)
        {
            var dbGame = await _databaseContext.Games.Include(g => g.Movements).FirstAsync(g => g.Id == gameId);
            var movementDb = dbGame.Movements.FirstOrDefault(m => m.Id == movement.Id);
            var dateTimeNow = DateTime.Now;

            if(movementDb == null)
            {
                //Create a new movement and add it to the game
                movementDb = movement.ConvertToModelDb();
                movementDb.CreationDate = dateTimeNow;
                dbGame.Movements.Add(movementDb);
            }
            else
            {
                //update existing movement
                movementDb.PlayerId = movement.PlayerId;
                movementDb.X = movement.X;
                movementDb.Y = movement.Y;
            }

            movementDb.LastChangeDate = dateTimeNow;
            dbGame.LastChangeDate = dateTimeNow;
            dbGame.Version = gameVersion;

            await _databaseContext.SaveChangesAsync();
        }

        #endregion
    }
}
