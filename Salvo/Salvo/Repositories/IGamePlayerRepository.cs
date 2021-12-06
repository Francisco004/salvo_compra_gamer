using Salvo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Salvo.Repositories
{
    public interface IGamePlayerRepository
    {
        public GamePlayer GetGamePlayerView(int idGamePlayer);

        void Save(GamePlayer gamePlayer);

        public GamePlayer FindById(long id);
    }
}