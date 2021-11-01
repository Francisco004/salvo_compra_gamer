using Microsoft.EntityFrameworkCore;
using Salvo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Salvo.Repositories
{
    public class GameRepository : RepositoryBase<Game>, IGameRepository
    {
        public GameRepository(SalvoContext salvoContext) : base(salvoContext)
        {
            
        }

        public IEnumerable<Game> GetAllGames()
        {
            return this.FindAll().OrderBy(game => game.CreationDate).ToList();
        }

        /*
        public IEnumerable<Game> GetAllGamesWithPlayers()
        {
            return this.FindAll(source => source.Incluide(game => game.));;
        }
        */
    }
}