using Microsoft.AspNetCore.Mvc;
using Salvo.Models;
using Salvo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Salvo.Controllers
{
    [Route("api/gamePlayers")]
    [ApiController]
    public class GamePlayersController : ControllerBase
    {
        private IGamePlayerRepository _repository;

        public GamePlayersController(IGamePlayerRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public IActionResult GetGameView(int id)
        {
            try
            {
                var gameplayer = _repository.GetGamePlayerView(id);

                var GameView = new GameViewDTO();
                GameView.Id = gameplayer.Id;
                GameView.CreationDate = gameplayer.Game.CreationDate;
                GameView.GamePlayers = new List<GamePlayerDTO>();

                foreach (var gp in gameplayer.Game.GamePlayers)
                {
                    var gpDTO = new GamePlayerDTO
                    {
                      Id = gp.Id,
                      JoinDate = gp.JoinDate,
                      Player = new PlayerDTO { Id = gp.Player.Id, Email = gp.Player.Email },
                    };

                    GameView.GamePlayers.Add(gpDTO);
                }

                GameView.Ships = new List<ShipDTO>();

                foreach (var ship in gameplayer.Ships)
                {
                   var shipDTO = new ShipDTO
                   {
                       Id = ship.Id,
                       Type = ship.Type,
                       Locations = ship.Locations.Select(location => new ShipLocationDTO { Id = location.Id, Location = location.Location }).ToList()
                   };

                       GameView.Ships.Add(shipDTO);
                };

                return Ok(GameView);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error " + ex.Message);
            }
        }
    }
}
