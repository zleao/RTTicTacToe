using System.Collections.Generic;
using System.Linq;
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
                Movements = gameDto.Movements.ConvertToModelDb(),
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

        public static Movement ConvertToModelDb(this MovementDto movementDto)
        {
            if (movementDto == null)
            {
                return null;
            }

            return new Movement
            {
                Id = movementDto.Id,
                PlayerId = movementDto.PlayerId,
                X = movementDto.X,
                Y = movementDto.Y
            };
        }

        public static ICollection<Movement> ConvertToModelDb(this IList<MovementDto> movements)
        {
            if (movements == null)
            {
                return null;
            }

            return movements.Select(m => m.ConvertToModelDb()).ToList();
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
                Movements = game.Movements.ConvertToModelDto(),
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

        public static MovementDto ConvertToModelDto(this Movement movement)
        {
            if (movement == null)
            {
                return null;
            }

            return new MovementDto(movement.Id, movement.PlayerId, movement.X, movement.Y);
        }

        public static IList<MovementDto> ConvertToModelDto(this ICollection<Movement> movements)
        {
            if (movements == null)
            {
                return null;
            }

            return movements.Select(m => m.ConvertToModelDto()).ToList();
        }

        #endregion
    }
}
