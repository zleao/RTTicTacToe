using RTTicTacToe.CQRS;
using RTTicTacToe.Events;
using RTTicTacToe.Queries.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTTicTacToe.Queries
{
    public class GameQueries : IGameQueries,
                               ISubscribeTo<GameCreated>,
                               ISubscribeTo<PlayerRegistered>,
                               ISubscribeTo<MovementMade>
    {
        #region Fields

        private readonly Dictionary<Guid, Game> _allGames = new Dictionary<Guid, Game>();

        #endregion

        #region Queries

        public List<Game> GetAllGames()
        {
            lock (_allGames)
            {
                return _allGames.Values.ToList();
            }
        }

        public Game GetGameById(Guid id)
        {
            return GetGame(id);
        }

        public List<Game> GetGamesOfPlayer(Guid playerId)
        {
            lock (_allGames)
            {
                return _allGames.Select(v => v.Value)
                                .Where(g => g.Player1?.Id == playerId || g.Player2?.Id == playerId)
                                .ToList();
            }
        }

        public List<Movement> GetMovementsFromGame(Guid gameId)
        {
            return GetGame(gameId).Movements;
        }

        public List<Player> GetPlayersFromGame(Guid gameId)
        {
            var game = GetGame(gameId);
            lock (game)
            {
                return new List<Player> { game.Player1, game.Player2 };
            }
        }

        #endregion

        #region Event Handlers

        public void Handle(GameCreated e)
        {
            lock (_allGames)
            {
                _allGames.Add(e.Id, new Game(e.Id, e.Name));
            }
        }

        public void Handle(PlayerRegistered e)
        {
            var game = GetGame(e.Id);
            lock (game)
            {
                var newPlayer = new Player(e.PlayerId, e.PlayerName);

                if (e.PlayerNumber == 1)
                {
                    game.Player1 = newPlayer;
                }
                else if (e.PlayerNumber == 2)
                {
                    game.Player2 = newPlayer;
                }
            }
        }

        public void Handle(MovementMade e)
        {
            var game = GetGame(e.Id);
            lock (game)
            {
                game.Movements.Add(new Movement
                {
                    PlayerId = e.PlayerId,
                    X = e.X,
                    Y = e.Y
                });
            }
        }

        #endregion

        #region Methods

        private Game GetGame(Guid id)
        {
            lock (_allGames)
            {
                return _allGames[id];
            }
        }

        #endregion
    }
}
