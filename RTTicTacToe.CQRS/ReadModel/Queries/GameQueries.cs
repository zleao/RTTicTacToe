using System;
using System.Collections.Generic;
using System.Linq;
using RTTicTacToe.CQRS.ReadModel.Dtos;
using RTTicTacToe.CQRS.ReadModel.Infrastructure;
using RTTicTacToe.CQRS.Utilities;

namespace RTTicTacToe.CQRS.ReadModel.Queries
{
    public class GameQueries : IGameQueries
    {
        #region Queries

        public List<GameDto> GetAllGames()
        {
            return InMemoryDatabase.AllGames.Values.ToList();
        }

        public GameDto GetGameById(Guid id)
        {
            return GameHelper.GetGame(id);
        }

        public List<GameDto> GetGamesOfPlayer(Guid playerId)
        {
            return InMemoryDatabase.AllGames.Select(v => v.Value)
                .Where(g => g.Player1?.Id == playerId || g.Player2?.Id == playerId)
                .ToList();
        }

        public List<MovementDto> GetMovementsFromGame(Guid gameId)
        {
            return GameHelper.GetGame(gameId).Movements;
        }

        public List<PlayerDto> GetPlayersFromGame(Guid gameId)
        {
            var game = GameHelper.GetGame(gameId);
            lock (game)
            {
                return new List<PlayerDto> { game.Player1, game.Player2 };
            }
        }

        #endregion
    }
}
