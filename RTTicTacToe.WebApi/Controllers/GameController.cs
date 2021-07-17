using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSlite.Commands;
using Microsoft.AspNetCore.Mvc;
using RTTicTacToe.CQRS.ReadModel.Dtos;
using RTTicTacToe.CQRS.ReadModel.Queries;
using RTTicTacToe.CQRS.WriteModel.Commands;
using RTTicTacToe.CQRS.WriteModel.Domain.Exceptions;
using RTTicTacToe.WebApi.Extensions;
using RTTicTacToe.WebApi.Models;

namespace RTTicTacToe.WebApi.Controllers
{
    /// <summary>
    /// COntroler to handle Game related calls
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly ICommandSender _commandSender;
        private readonly IGameQueries _readModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RTTicTacToe.WebApi.Controllers.GameController"/> class.
        /// </summary>
        /// <param name="commandSender">Command sender.</param>
        /// <param name="readModel">Read model.</param>
        public GameController(ICommandSender commandSender, IGameQueries readModel)
        {
            _commandSender = commandSender;
            _readModel = readModel;
        }

        /// <summary>
        /// About this instance.
        /// </summary>
        /// <returns>The about.</returns>
        [HttpGet("about")]
        public ActionResult About()
        {
            return Content($"RTTicTacToe.WebApi v1.0.0");
        }

        // GET api/game
        /// <summary>
        /// Gets all games.
        /// </summary>
        /// <returns>List with all the existing games</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GameDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetAllGames()
        {
            return Ok(await _readModel.GetAllGamesAsync());
        }

        // GET api/game/<id>        
        /// <summary>
        /// Gets a game by an identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Game with the requested identifier</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GameDto), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<GameDto>> GetGameById(Guid id)
        {
            return Ok(await _readModel.GetGameByIdAsync(id));
        }

        //POST api/game        
        /// <summary>
        /// Creates a new game.
        /// </summary>
        /// <param name="name">The name.</param>
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
                return StatusCode(500, ex.ExceptionToString());
            } 
        }

        //POST api/game/<id>/player                     
        /// <summary>
        /// Adds a player to a game.
        /// </summary>
        /// <param name="id">The game identifier.</param>
        /// <param name="model">The add player model.</param>
        [HttpPost("{id}/player")]
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
                return StatusCode(500, ex.ExceptionToString());
            } 
        }

        //POST api/game/<id>/movement        
        /// <summary>
        /// Makes a movement.
        /// </summary>
        /// <param name="id">The game identifier.</param>
        /// <param name="model">The make movement model.</param>
        [HttpPost("{id}/movements")]
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
                return StatusCode(500, ex.ExceptionToString());
            } 
        }

        // GET api/game/<id>/events        
        /// <summary>
        /// Gets the game board.
        /// </summary>
        /// <param name="id">The game identifier.</param>
        /// <returns>Integer array with the game board definition</returns>
        [HttpGet("{id}/board")]
        [ProducesResponseType(typeof(IEnumerable<int[,]>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<int[,]>>> GetGameBoard([FromRoute]Guid id)
        {
            return Ok(await _readModel.GetGameBoardAsync(id));
        }

        // GET api/game/<id>/events        
        /// <summary>
        /// Gets the game events.
        /// </summary>
        /// <param name="id">The game identifier.</param>
        /// <returns>List with the game events</returns>
        [HttpGet("{id}/events")]
        [ProducesResponseType(typeof(IEnumerable<EventDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetGameEvents([FromRoute]Guid id)
        {
            return Ok(await _readModel.GetGameEventsAsync(id));
        }
    }
}
