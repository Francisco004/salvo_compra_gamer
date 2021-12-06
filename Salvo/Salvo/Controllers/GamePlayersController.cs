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
        private readonly IScoreReposirory _scoreReposirory;

        public GamePlayersController(IGamePlayerRepository repository, IScoreReposirory scoreReposirory)
        {
            _repository = repository;
            _scoreReposirory = scoreReposirory;
        }

        [HttpGet("{id}", Name = "GetGameView")]
        public IActionResult GetGameView(int id)
        {
            try
            {
                var email = User.FindFirst("Player") != null ? User.FindFirst("Player").Value : "Guest";
                var gameplayer = _repository.GetGamePlayerView(id);
                if (gameplayer.Player.Email == email)
                {
                    var gameView = new GameViewDTO();
                    {
                        gameView.Id = gameplayer.Id;
                        gameView.CreationDate = gameplayer.Game.CreationDate;
                        gameView.GamePlayers = new List<GamePlayerDTO>();

                    }

                    foreach (var gp in gameplayer.Game.GamePlayers)
                    {
                        var gpDTO = new GamePlayerDTO
                        {
                            Id = gp.Id,
                            JoinDate = gp.JoinDate,
                            Player = new PlayerDTO { Id = gp.Player.Id, Email = gp.Player.Email, Name = gp.Player.Name }
                        };
                        gameView.GamePlayers.Add(gpDTO);
                    }

                    gameView.Ships = new List<ShipDTO>();
                    foreach (var ship in gameplayer.Ships)
                    {
                        var shipDTO = new ShipDTO
                        {
                            Id = ship.Id,
                            Type = ship.Type,
                            Locations = ship.Locations.Select(location => new ShipLocationDTO { Id = location.Id, Location = location.Location }).ToList()
                        };
                        gameView.Ships.Add(shipDTO);
                    };

                    gameView.Salvos = gameplayer.Game.GamePlayers.SelectMany(gps => gps.Salvos.Select(salvo => new SalvoDTO
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

                    gameView.Hits = gameplayer.GetHits();

                    gameView.HitsOpponent = gameplayer.GetOpponent()?.GetHits();

                    gameView.Sunks = gameplayer.GetSunks();

                    gameView.SunksOpponent = gameplayer.GetOpponent()?.GetSunks();

                    gameView.GameState = Enum.GetName(typeof(GameState), gameplayer.GetGameState());

                    return Ok(gameView);
                }
                else
                {
                    return Forbid();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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
                var gameplayer = _repository.FindById(id);

                if (gameplayer == null)
                {
                    return StatusCode(403, "No existe el juego");
                }
                else if (gameplayer.Player.Email != User.FindFirst("Player").Value)
                {
                    return StatusCode(403, "El usuario no se encuentra en el juego.");
                }
                else if (gameplayer.Game.GamePlayers.Count != 2)
                {
                    return StatusCode(403, "El juego no ha iniciado aun.");
                }
                else if (gameplayer.Ships.Count == 0)
                {
                    return StatusCode(403, "El Usuario logueado no ha posicionado los barcos.");
                }

                GameState gameState = gameplayer.GetGameState();

                if (gameState == GameState.LOSS || gameState == GameState.WIN || gameState == GameState.TIE)
                {
                    return StatusCode(403, "El juego ha terminado");
                }

                GamePlayer opponent = gameplayer.GetOpponent();

                if (opponent.Ships.Count == 0)
                {
                    return StatusCode(403, "El oponente no ha posicionado los barcos.");
                }

                int playerTurn = gameplayer.Salvos != null ? gameplayer.Salvos.Count + 1 : 1;
                int opponentTurn = opponent.Salvos != null ? opponent.Salvos.Count : 0;

                if ((playerTurn - opponentTurn) < -1 || (playerTurn - opponentTurn) > 1)
                {
                    return StatusCode(403, "No se puede adelantar el turno.");
                }

                gameplayer.Salvos.Add(new Models.Salvo
                {
                    GamePlayerID = gameplayer.Id,
                    Turn = playerTurn,
                    Locations = salvo.Locations.Select(location => new SalvoLocation { Location = location.Location }).ToList()
                });

                _repository.Save(gameplayer);

                gameState = gameplayer.GetGameState();

                if (gameState == GameState.WIN)
                {
                    var score = new Score
                    {
                        FinishDate = DateTime.Now,
                        GameId = gameplayer.GameId,
                        PlayerId = gameplayer.PlayerId,
                        Point = 1
                    };
                    _scoreReposirory.Save(score);

                    var scoreOpponent = new Score
                    {
                        FinishDate = DateTime.Now,
                        GameId = gameplayer.GameId,
                        PlayerId = opponent.PlayerId,
                        Point = 0
                    };
                    _scoreReposirory.Save(scoreOpponent);
                }
                else if (gameState == GameState.LOSS)
                {
                    var score = new Score
                    {
                        FinishDate = DateTime.Now,
                        GameId = gameplayer.GameId,
                        PlayerId = gameplayer.PlayerId,
                        Point = 0
                    };
                    _scoreReposirory.Save(score);

                    var scoreOpponent = new Score
                    {
                        FinishDate = DateTime.Now,
                        GameId = gameplayer.GameId,
                        PlayerId = opponent.PlayerId,
                        Point = 1
                    };
                    _scoreReposirory.Save(scoreOpponent);
                }
                else if (gameState == GameState.TIE)
                {
                    var score = new Score
                    {
                        FinishDate = DateTime.Now,
                        GameId = gameplayer.GameId,
                        PlayerId = gameplayer.PlayerId,
                        Point = 0.5
                    };
                    _scoreReposirory.Save(score);

                    var scoreOpponent = new Score
                    {
                        FinishDate = DateTime.Now,
                        GameId = gameplayer.GameId,
                        PlayerId = opponent.PlayerId,
                        Point = 0.5
                    };
                    _scoreReposirory.Save(scoreOpponent);
                }

                return StatusCode(201, gameplayer.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}


