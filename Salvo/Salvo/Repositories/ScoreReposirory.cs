using Salvo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Repositories
{
    public class ScoreReposirory : RepositoryBase<Score>, IScoreReposirory
    {
        public ScoreReposirory(SalvoContext reposiroryContext) : base (reposiroryContext)
        {
        
        }

        public void Save(Score score)
        {
            Create(score);
            SaveChanges();
        }
    }
}
