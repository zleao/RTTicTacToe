using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RTTicTacToe.Forms.Models;
using RTTicTacToe.WebApi.Models;

namespace RTTicTacToe.Forms.Services
{
    public class GameService : IGameService
    {
        HttpClient client;
        IEnumerable<Game> _games;

        public GameService()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri($"{App.AzureBackendUrl}/")
            };

            _games = new List<Game>();
        }

        public async Task<IEnumerable<Game>> GetGamesAsync(bool forceRefresh = false)
        {
            if (forceRefresh)
            {
                var json = await client.GetStringAsync($"api/game");
                _games = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Game>>(json));
            }

            return _games;
        }

        public async Task<bool> AddGameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            var serializedItem = JsonConvert.SerializeObject(name);

            var response = await client.PostAsync($"api/game", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<Game> GetGameAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return null;
            }

            var json = await client.GetStringAsync($"api/game/{id}");
            return await Task.Run(() => JsonConvert.DeserializeObject<Game>(json));
        }

        public async Task<bool> AddPlayerAsync(Guid gameId, int gameVersion, Guid playerId, string playerName)
        {
            if (gameId == Guid.Empty ||
                gameVersion < 1 ||
                playerId == Guid.Empty ||
                string.IsNullOrEmpty(playerName))
            {
                return false;
            }

            var serializedItem = JsonConvert.SerializeObject(new AddPlayerModel
            {
                Version = gameVersion,
                PlayerId = playerId,
                PlayerName = playerName
            });

            var response = await client.PostAsync($"api/game/{gameId}/player", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> MakeMovementAsync(Guid gameId, int gameVersion, Guid playerId, short x, short y)
        {
            if (gameId == Guid.Empty ||
                gameVersion < 1 ||
                playerId == Guid.Empty ||
                x < 0 || x > 2 ||
                y < 0 || y > 2)
            {
                return false;
            }

            var serializedItem = JsonConvert.SerializeObject(new MakeMovementModel
            {
                Version = gameVersion,
                PlayerId = playerId,
                X = x,
                Y = y
            });

            var response = await client.PostAsync($"api/game/{gameId}/movements", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<IList<Movement>> GetGameMovementsAsync(Guid gameId)
        {
            if (gameId == Guid.Empty)
            {
                return null;
            }

            var json = await client.GetStringAsync($"api/game/{gameId}/movements");
            return JsonConvert.DeserializeObject<List<Movement>>(json);
        }

        public async Task<IList<Event>> GetGameEventsAsync(Guid gameId)
        {
            if (gameId == Guid.Empty)
            {
                return null;
            }

            var json = await client.GetStringAsync($"api/game/{gameId}/events");
            return JsonConvert.DeserializeObject<List<Event>>(json);
        }

        public bool IsValidPlayer(Player player)
        {
            return player != null && player.Id != Guid.Empty && !string.IsNullOrEmpty(player.Name);
        }
    }
}