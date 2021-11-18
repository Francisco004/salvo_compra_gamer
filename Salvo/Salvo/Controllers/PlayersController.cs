using Microsoft.AspNetCore.Mvc;
using Salvo.Models;
using Salvo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Salvo.Controllers
{
    [Route("api/players")]
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
                string mensaje;
                bool validacionEmail = ValidateEmail(player.Email);
                var Existencia = _repository.FindByEmail(player.Email);
                bool validacionPassword = ValidatePassword(player.Password, out mensaje);
                
                if (Existencia == null)
                {
                    if (string.IsNullOrWhiteSpace(player.Email) && string.IsNullOrWhiteSpace(player.Password) && string.IsNullOrEmpty(player.Name)) 
                    { 
                        return StatusCode(403, "Datos inválidos"); 
                    }
                    else if(!validacionEmail)
                    {
                        return StatusCode(403, "Datos errones en el email");
                    }
                    else if (string.IsNullOrEmpty(player.Name))
                    {
                        return StatusCode(403, "Datos errones en el nombre");
                    }
                    else if (!validacionPassword)
                    {
                        return StatusCode(403, mensaje);
                    }
                    else
                    {
                        Player p = new Player
                        {
                            Email = player.Email,
                            Password = player.Password,
                            Name = player.Name
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

            bool ValidatePassword(string password, out string ErrorMessage)
            {
                var input = password;
                ErrorMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(input))
                {
                    ErrorMessage = "La contraseña no debe estar vacia...";
                    return false;
                }

                var hasNumber = new Regex(@"[0-9]+");
                var hasUpperChar = new Regex(@"[A-Z]+");
                var hasMiniMaxChars = new Regex(@".{8,15}");
                var hasLowerChar = new Regex(@"[a-z]+");
                var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

                if (!hasLowerChar.IsMatch(input))
                {
                    ErrorMessage = "La contraseña debe tener minimo un caracter en minuscula...";
                    return false;
                }
                else if (!hasUpperChar.IsMatch(input))
                {
                    ErrorMessage = "La contraseña debe tener minimo un caracter en mayuscula...";
                    return false;
                }
                else if (!hasMiniMaxChars.IsMatch(input))
                {
                    ErrorMessage = "La contraseña debe tener mas de 8 caracteres...";
                    return false;
                }
                else if (!hasNumber.IsMatch(input))
                {
                    ErrorMessage = "La contraseña debe tener al menos un numero...";
                    return false;
                }

                else if (!hasSymbols.IsMatch(input))
                {
                    ErrorMessage = "La contraseña debe tener al menos un caracter especial...";
                    return false;
                }
                else
                {
                    return true;
                }
            }

            bool ValidateEmail(string emailParam)
            {
                bool retorno = false;
                string email = emailParam;

                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(email);

                if (match.Success)
                {
                    retorno = true;
                }

                return retorno;
            }
        }
    }
}
