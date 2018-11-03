using System;
using System.Collections.Generic;
using RTTicTacToe.CQRS.ReadModel.Dtos;

namespace RTTicTacToe.CQRS.ReadModel.Queries
{
    public interface IGameQueries
    {
        List<GameDto> GetAllGames();
        GameDto GetGameById(Guid id);
        List<GameDto> GetGamesOfPlayer(Guid playerId);
        List<PlayerDto> GetPlayersFromGame(Guid gameId);
        List<MovementDto> GetMovementsFromGame(Guid gameId);
    }
}