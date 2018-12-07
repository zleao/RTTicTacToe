using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RTTicTacToe.CQRS.ReadModel.Dtos;
using RTTicTacToe.CQRS.ReadModel.Queries;

namespace RTTicTacToe.WebApi.Controllers
{
    /// <summary>
    /// Player controller.
    /// </summary>
    [Route("api/player")]
    [ApiController]
    public class PlayerController : Controller
    {
        private readonly IGameQueries _readModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RTTicTacToe.WebApi.Controllers.PlayerController"/> class.
        /// </summary>
        /// <param name="readModel">Read model.</param>
        public PlayerController(IGameQueries readModel)
        {
            _readModel = readModel;
        }

        // GET api/player/<id>/games        
        /// <summary>
        /// Gets the games where a player participated or is participating.
        /// </summary>
        /// <param name="id">The player identifier.</param>
        /// <returns></returns>
        [HttpGet("{id}/games")]
        [ProducesResponseType(typeof(IEnumerable<GameDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetPlayerGames([FromRoute]Guid id)
        {
            return Ok(await _readModel.GetPlayerGamesAsync(id));
        }
    }
}
