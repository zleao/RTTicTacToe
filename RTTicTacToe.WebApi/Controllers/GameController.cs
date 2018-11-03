using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSlite.Commands;
using Microsoft.AspNetCore.Mvc;
using RTTicTacToe.CQRS.ReadModel.Dtos;
using RTTicTacToe.CQRS.ReadModel.Queries;
using RTTicTacToe.CQRS.WriteModel.Commands;
using RTTicTacToe.CQRS.WriteModel.Domain.Exceptions;
using RTTicTacToe.WebApi.Models;

namespace RTTicTacToe.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly ICommandSender _commandSender;
        private readonly IGameQueries _readModel;

        public GameController(ICommandSender commandSender, IGameQueries readModel)
        {
            _readModel = readModel;
            _commandSender = commandSender;
        }

        // GET api/game
        /// <summary>
        /// Gets all games.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GameDto>), 200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<GameDto>> GetAllGames()
        {
            return _readModel.GetAllGames();
        }

        // GET api/game/<id>        
        /// <summary>
        /// Gets the game by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GameDto), 200)]
        [ProducesResponseType(400)]
        public ActionResult<GameDto> GetGameById(Guid id)
        {
            return _readModel.GetGameById(id);
        }

        //POST api/game        
        /// <summary>
        /// Creates a new game.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateNewGame([FromBody]string name)
        {
            try
            {
                await _commandSender.Send(new CreateGame(Guid.NewGuid(), name));
                return Ok();
            }
            catch (GameIdInvalidException)
            {
                return BadRequest(typeof(GameIdInvalidException).Name);
            }
            catch (GameNameInvalidException)
            {
                return BadRequest(typeof(GameNameInvalidException).Name);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            } 
        }

        //POST api/game/<id>                      
        /// <summary>
        /// Adds a player to a game.
        /// </summary>
        /// <param name="id">The game identifier.</param>
        /// <param name="model">The add player model.</param>
        /// <returns></returns>
        [HttpPost("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddPlayerToGame([FromRoute]Guid id, [FromBody]AddPlayerModel model)
        {
            try
            {
                await _commandSender.Send(new RegisterPlayer(id, model.PlayerId, model.PlayerName, model.Version));
                return Ok();
            }
            catch (GameAlreadyFinishedException)
            {
                return BadRequest(typeof(GameAlreadyFinishedException).Name);
            }
            catch (GameAlreadyStartedException)
            {
                return BadRequest(typeof(GameAlreadyStartedException).Name);
            }
            catch (PlayerIdInvalidException)
            {
                return BadRequest(typeof(PlayerIdInvalidException).Name);
            }
            catch (PlayerAlreadyRegisteredInTheGameException)
            {
                return BadRequest(typeof(PlayerAlreadyRegisteredInTheGameException).Name);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            } 
        }

        //POST api/game/<id>/movement        
        /// <summary>
        /// Makes a movement.
        /// </summary>
        /// <param name="id">The game identifier.</param>
        /// <param name="model">The make movement model.</param>
        /// <returns></returns>
        [HttpPost("{id}/movement")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> MakeMovement([FromRoute]Guid id, [FromBody]MakeMovementModel model)
        {
            try
            {
                await _commandSender.Send(new MakeMovement(id, model.PlayerId, model.X, model.Y, model.Version));
                return Ok();
            }
            catch (GameNotStartedException)
            {
                return BadRequest(typeof(GameNotStartedException).Name);
            }
            catch (GameAlreadyFinishedException)
            {
                return BadRequest(typeof(GameAlreadyFinishedException).Name);
            }
            catch (MovementInvalidPlayerIdException)
            {
                return BadRequest(typeof(MovementInvalidPlayerIdException).Name);
            }
            catch (MovementOutOfBoundsException)
            {
                return BadRequest(typeof(MovementOutOfBoundsException).Name);
            }
            catch (MovementAlreadyTakenException)
            {
                return BadRequest(typeof(MovementAlreadyTakenException).Name);
            }
            catch (MovementWrongPlayerException)
            {
                return BadRequest(typeof(MovementWrongPlayerException).Name);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            } 
        }
    }
}
