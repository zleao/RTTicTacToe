using Microsoft.AspNetCore.Mvc;
using RTTicTacToe.Commands;
using RTTicTacToe.Queries.Models;
using RTTicTacToe.WebApp.ActionFilters;
using System;

namespace RTTicTacToe.WebApp.Controllers
{
    [IncludeLayoutData]
    public class GameController : Controller
    {
        private static Guid? _currentGameId;

        public IActionResult Index()
        {
            _currentGameId = null;

            return View();
        }

        public IActionResult About()
        {
            _currentGameId = null;

            return View();
        }

        public IActionResult Error()
        {
            _currentGameId = null;

            return View();
        }

        public IActionResult CreateGame()
        {
            _currentGameId = null;

            return View();
        }

        [HttpPost]
        public IActionResult CreateGame(string name)
        {
            var cmd = new CreateGame
            {
                Id = Guid.NewGuid(),
                Name = name
            };

            Domain.Dispatcher.SendCommand(cmd);
            
            _currentGameId = null;

            return View("Index");
        }

        public IActionResult GameStatus(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                NotFound();
            }

            _currentGameId = id;

            return View("Index", Domain.GameQueries.GetGameById(id.Value));
        }

        public IActionResult CreatePlayer(int number)
        {
            ViewData["PlayerNumber"] = number;
            return View();
        }

        [HttpPost]
        public IActionResult CreatePlayer(string name, int number)
        {
            var cmd = new RegisterPlayer
            {
                Id = _currentGameId.Value,
                PlayerId = Guid.NewGuid(),
                PlayerName = name,
                PlayerNumber = number
            };

            Domain.Dispatcher.SendCommand(cmd);

            return View("Index", Domain.GameQueries.GetGameById(_currentGameId.Value));
        }

        [HttpPost]
        public IActionResult AddMovement(int x, int y)
        {
            var playerId = Guid.Parse(Request.Form["player"]);

            var cmd = new MakeMovement
            {
                Id = _currentGameId.Value,
                PlayerId = playerId,
                X = x,
                Y = y
            };

            Domain.Dispatcher.SendCommand(cmd);

            return View("Index", Domain.GameQueries.GetGameById(_currentGameId.Value));
        }
    }
}
