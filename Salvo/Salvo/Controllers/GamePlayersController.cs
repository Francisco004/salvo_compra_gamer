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
        private readonly IPlayerRepository _playerRepository;

        public GamePlayersController(IGamePlayerRepository repository, IPlayerRepository playerrepository)
        {
            _repository = repository;
            _playerRepository = playerrepository;
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
        public IActionResult Post(long id, [FromBody] List<ShipDTO> ship)
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
                    return StatusCode(403, "El usuario no se encuentra en el juego");
                }

                if (game.Ships.Count == 5)
                {
                    return StatusCode(403, "Ya se han posicionado los barcos");
                }

                game.Ships = ship.Select(ship => new Ship
                {
                    GamePlayerId = game.Id,
                    Type = ship.Type,
                    Locations = ship.Locations.Select(location => new ShipLocation { ShipId = ship.Id, Location = location.Location }).ToList()
                }).ToList();

                _repository.Save(game);

                return StatusCode(201, "Created");
            }
            catch (Exception ex)
            {
                return StatusCode(403, ex.Message);
            }
        }

        [HttpPost("{id}/salvos")]
        public IActionResult Post(long id, [FromBody] SalvoDTO salvo)
        {
            try
            {
                GamePlayer gamePlayer = _repository.FindById(id);
                
                if(gamePlayer == null)
                {
                    return StatusCode(403, "No existe el juego.");
                }
                else if (gamePlayer.Player.Email != User.FindFirst("Player").Value)
                {
                    return StatusCode(403, "El usuario no se encuentra en el juego.");
                }

                GamePlayer opponentGamePlayer = gamePlayer.GetOpponent();

                if (gamePlayer.Game.GamePlayers.Count != 2)
                {
                    return StatusCode(403, "No hay a quien disparar.");
                }

                if(gamePlayer.Ships.Count == 0)
                {
                    return StatusCode(403, "El usuario logeado no ha pisicionado los barcos");
                }

                if (opponentGamePlayer.Ships.Count == 0)
                {
                    return StatusCode(403, "El oponente no ha pisicionado los barcos");
                }

                int playerTurn = gamePlayer.Salvos != null ? gamePlayer.Salvos.Count + 1 : 1;
                int opponentTurn = opponentGamePlayer.Salvos != null ? opponentGamePlayer.Salvos.Count : 0;

                if ((playerTurn - opponentTurn) < -1 || (playerTurn - opponentTurn) > 1)
                {
                    return StatusCode(500, "No se puede adelantar el turno");
                }

                gamePlayer.Salvos.Add(new Models.Salvo
                {
                    Turn = playerTurn,
                    GamePlayerID = gamePlayer.Id,
                    Locations = salvo.Locations.Select(location => new SalvoLocation 
                    { 
                        Location = location.Location
                    }).ToList()
                });

                _repository.Save(gamePlayer);

                return StatusCode(201, gamePlayer.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}


