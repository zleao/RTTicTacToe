using Newtonsoft.Json;
using RTTicTacToe.CQRS.Database.Models;
using RTTicTacToe.CQRS.ReadModel.Dtos;

namespace RTTicTacToe.CQRS.ReadModel.Infrastructure
{
    public static class ModelsConverter
    {
        #region To ModelDb Converters

        public static Game ConvertToModelDb(this GameDto gameDto)
        {
            if(gameDto == null)
            {
                return null;
            }

            return new Game
            {
                CreationDate = gameDto.CreationDate,
                Id = gameDto.Id,
                LastChangeDate = gameDto.LastChangeDate,
                Name = gameDto.Name,
                Player1 = gameDto.Player1.ConvertToModelDb(),
                Player2 = gameDto.Player2.ConvertToModelDb(),
                Version = gameDto.Version,
                BoardJsonString = JsonConvert.SerializeObject(gameDto.Board),
                Winner = gameDto.Winner.ConvertToModelDb()
            };
        }

        public static Player ConvertToModelDb(this PlayerDto playerDto)
        {
            if (playerDto == null)
            {
                return null;
            }

            return new Player
            {
                Id = playerDto.Id,
                Name = playerDto.Name 
            };
        }

        #endregion

        #region To ModelDto Converters

        public static GameDto ConvertToModelDto(this Game game)
        {
            if(game == null)
            {
                return null;
            }

            return new GameDto(game.Id, game.Name, game.Version)
            {
                CreationDate = game.CreationDate,
                LastChangeDate = game.LastChangeDate,
                Board = JsonConvert.DeserializeObject<int[,]>(game.BoardJsonString ?? string.Empty) ?? new int[3,3],
                Player1 = game.Player1.ConvertToModelDto(),
                Player2 = game.Player2.ConvertToModelDto(),
                Winner = game.Winner.ConvertToModelDto()
            };
        }

        public static PlayerDto ConvertToModelDto(this Player player)
        {
            if (player == null)
            {
                return null;
            }

            return new PlayerDto(player.Id, player.Name);
        }

        #endregion
    }
}
