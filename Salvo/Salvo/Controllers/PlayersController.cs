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
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private IPlayerRepository _repository;
        public PlayersController(IPlayerRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}", Name = "GetPlayer")] 
        public string Get(int id) 
        { 
            return "value"; 
        }

        [HttpPost]
        public IActionResult Post([FromBody] PlayerDTO player)
        {
            try
            {
                var Existencia = _repository.FindByEmail(player.Email);

                if(Existencia == null)
                {
                    if (string.IsNullOrWhiteSpace(player.Email) || string.IsNullOrWhiteSpace(player.Password)) 
                    { 
                        return StatusCode(403, "Datos inválidos"); 
                    }
                    else
                    {
                        Player p = new Player
                        {
                            Email = player.Email,
                            Password = player.Password
                        };

                        _repository.Save(p);

                        return StatusCode(201, "El usuario se guardo con exito!");
                    }
                }
                else
                {
                    return StatusCode(403, "Email en uso...");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
