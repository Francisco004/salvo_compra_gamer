﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Salvo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Salvo.Repositories
{
    public class GamePlayerRepository : RepositoryBase<GamePlayer>, IGamePlayerRepository
    {
        public GamePlayerRepository(SalvoContext salvoContext) : base(salvoContext)
        {

        }

        public GamePlayer GetGamePlayerView(int idGamePlayer)
        {
            return FindAll(source => source.Include(gamePlayer => gamePlayer.Ships)
                                               .ThenInclude(ship => ship.Locations)
                                               .Include(gamePlayer => gamePlayer.Game)
                                               .ThenInclude(game => game.GamePlayers)
                                               .ThenInclude(gp => gp.Player))
                                               .Where(gamePlayer => gamePlayer.Id == idGamePlayer)
                                               .OrderBy(game => game.JoinDate)
                                               .FirstOrDefault();
        }
    }
}