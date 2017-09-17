using RTTicTacToe.Queries.Models;
using System;
using System.Collections.Generic;

namespace RTTicTacToe.Queries
{
    public interface IGameQueries
    {
        List<Game> GetAllGames();
        Game GetGameById(Guid id);
        List<Game> GetGamesOfPlayer(Guid playerId);
        List<Player> GetPlayersFromGame(Guid gameId);
        List<Movement> GetMovementsFromGame(Guid gameId);
    }
}
