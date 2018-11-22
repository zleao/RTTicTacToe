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

        public async Task AddGameAsync(GameDto game)
        {
            await _databaseContext.Game.AddAsync(new Database.Models.Game
            {
                Id = game.Id,
                Name = game.Name,
                Version = game.Version,
                CreationDate = DateTime.Now.Date,
                LastChangeDate = DateTime.Now.Date
            });

            await _databaseContext.SaveChangesAsync();
        }

        public async Task<IList<GameDto>> GetAllGamesAsync()
        {
            return await _databaseContext.Game.Select(g => new GameDto(g.Id, g.Name, g.Version)).ToListAsync();
        }

        public async Task<GameDto> GetGameByIdAsync(Guid id)
        {
            var game = await _databaseContext.Game.FindAsync(id);
            if(game != null)
            {
                return new GameDto(game.Id, game.Name, game.Version);    
            }

            return null;
        }

        public Task<IList<GameDto>> GetGamesByPlayerIdAsync(Guid playerId)
        {
            return GetAllGamesAsync();
        }
    }
}
