﻿using Microsoft.AspNetCore.Mvc;
using Salvo.Models;
using Salvo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Salvo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private IGameRepository _repository;

        public GamesController(IGameRepository repository)
        {
            _repository = repository;
        }

        // GET: api/<GamesController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var games = _repository.GetAllGamesWithPlayers().Select(newGame => new GameDTO { Id = newGame.Id, CreationDate = newGame.CreationDate, GamePlayers = newGame.GamePlayers.Select(newGamePlayerDTO => new GamePlayerDTO { Id = newGamePlayerDTO.Id, JoinDate = newGamePlayerDTO.JoinDate, Player = new PlayerDTO { Id = newGamePlayerDTO.Player.Id, Email = newGamePlayerDTO.Player.Email } }).ToList()}).ToList();
                return Ok(games);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
