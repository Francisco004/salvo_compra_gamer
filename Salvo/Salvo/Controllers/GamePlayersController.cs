using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Salvo.Models;
using Salvo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Salvo.Controllers
{
    [Route("api/gamePlayers")]
    [ApiController]
    [Authorize("PlayerOnly")]
    public class GamePlayersController : ControllerBase
    {
        private readonly IGamePlayerRepository _repository;

        public GamePlayersController(IGamePlayerRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}", Name = "GetGameView")]
        public IActionResult GetGameView(int id)
        {
            try
            {
                string email = User.FindFirst("Player") != null ? User.FindFirst("Player").Value : "Guest";

                var gameplayer = _repository.GetGamePlayerView(id);

                if(gameplayer.Player.Email != email)
                {
                    return Forbid();
                }

                var GameView = new GameViewDTO
                {
                    Id = gameplayer.Id,
                    CreationDate = gameplayer.Game.CreationDate,
                    GamePlayers = new List<GamePlayerDTO>()
                };

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

                GameView.Salvos = gameplayer.Game.GamePlayers.SelectMany(gps => gps.Salvos.Select(salvo => new SalvoDTO
                {
                    Id = salvo.Id,
                    Turn = salvo.Turn,
                    Player = new PlayerDTO
                    {
                        Id = gps.Player.Id,
                        Email = gps.Player.Email
                    },
                    Locations = salvo.Locations.Select(salvoLocation => new SalvoLocationDTO
                    {
                        Id = salvoLocation.Id,
                        Location = salvoLocation.Location
                    }).ToList()
                })).ToList();

                return Ok(GameView);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error " + ex.Message);
            }
        }

        [HttpPost("{id}/ships")]
        public IActionResult Post(long id, [FromBody] List<ShipDTO> barcos)
        {
            try
            {
                GamePlayer game = _repository.FindById(id);

                if (game == null)
                {
                    return StatusCode(403, "No existe el juego");
                }

                if (game.Player.Email != User.FindFirst("Player").Value)
                {
                    return StatusCode(403, "Ya se han posicionado los barcos");
                }

                if (game.Ships.Count == 5)
                {
                    return StatusCode(403, "Ya se han posicionado los barcos");
                }

                List<Ship> newShips = new();

                foreach(var shipDTO in barcos)
                {
                    Ship s = new()
                    {
                        Type = shipDTO.Type,
                        Locations = shipDTO.Locations.Select(shipLocation => new ShipLocation { Location = shipLocation.Location }).ToList()
                    };

                    newShips.Add(s);
                }

                //game.Ships = (ICollection<Ship>)barcos;
                game.Ships = newShips;
                _repository.Save(game);

                return StatusCode(201, "Created");
            }
            catch (Exception ex)
            {
                return StatusCode(403, ex.Message);
            }
        }
    }
}


