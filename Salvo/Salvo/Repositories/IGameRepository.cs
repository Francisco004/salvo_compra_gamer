using Salvo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Salvo.Repositories
{
    public interface IGameRepository
    {
        IEnumerable<Game> GetAllGames();
        //IEnumerable<Game> GetAllGamesWithPlayers();
    }
}