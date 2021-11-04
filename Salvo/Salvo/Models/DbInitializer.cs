using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Models
{
    public static class DbInitializer
    {
        public static void Initialize(SalvoContext context)
        {
            #region Player
            if (!context.Players.Any())
            {
                var players = new Player[]
                {
                    new Player{Name = "Jack Bauer "  ,  Email = "j.bauer@ctu.gov"    , Password = "24"  },
                    new Player{Name = "Chloe O'Brian",  Email = "c.obrian@ctu.gov"   , Password = "42"  },
                    new Player{Name = "Kim Bauer"    ,  Email = "kim_bauer@gmail.com", Password = "kb"  },
                    new Player{Name = "Tony Almeida ",  Email = "t.almeida@ctu.gov"  , Password = "mole"},
                };

                foreach (Player p in players)
                {
                    context.Players.Add(p);
                }
            }
            #endregion

            #region Games
            if (!context.Games.Any())
            {
                DateTime fecha = DateTime.Now;

                var games = new Game[]
                {
                    new Game{CreationDate = fecha.AddHours(0)},
                    new Game{CreationDate = fecha.AddHours(1)},
                    new Game{CreationDate = fecha.AddHours(2)},
                    new Game{CreationDate = fecha.AddHours(3)},
                    new Game{CreationDate = fecha.AddHours(4)},
                    new Game{CreationDate = fecha.AddHours(5)},
                    new Game{CreationDate = fecha.AddHours(6)},
                    new Game{CreationDate = fecha.AddHours(7)},
                };

                foreach (Game g in games)
                {
                    context.Games.Add(g);
                }
            }
            #endregion

            #region GamePlayers
            if (!context.GamePlayers.Any())
            {
                var GamePlayers = new GamePlayer[]
                {
                    new GamePlayer{ Game = context.Games.Find(1L), JoinDate = DateTime.Now, Player = context.Players.Find(1L)}, //1
                    new GamePlayer{ Game = context.Games.Find(1L), JoinDate = DateTime.Now, Player = context.Players.Find(2L)}, //2
                    new GamePlayer{ Game = context.Games.Find(2L), JoinDate = DateTime.Now, Player = context.Players.Find(1L)}, //3
                    new GamePlayer{ Game = context.Games.Find(2L), JoinDate = DateTime.Now, Player = context.Players.Find(2L)}, //4
                    new GamePlayer{ Game = context.Games.Find(3L), JoinDate = DateTime.Now, Player = context.Players.Find(2L)}, //5
                    new GamePlayer{ Game = context.Games.Find(3L), JoinDate = DateTime.Now, Player = context.Players.Find(4L)}, //6
                    new GamePlayer{ Game = context.Games.Find(4L), JoinDate = DateTime.Now, Player = context.Players.Find(2L)}, //7
                    new GamePlayer{ Game = context.Games.Find(4L), JoinDate = DateTime.Now, Player = context.Players.Find(1L)}, //8
                    new GamePlayer{ Game = context.Games.Find(5L), JoinDate = DateTime.Now, Player = context.Players.Find(4L)}, //9
                    new GamePlayer{ Game = context.Games.Find(5L), JoinDate = DateTime.Now, Player = context.Players.Find(1L)}, //10
                    new GamePlayer{ Game = context.Games.Find(6L), JoinDate = DateTime.Now, Player = context.Players.Find(3L)}, //11
                    new GamePlayer{ Game = context.Games.Find(7L), JoinDate = DateTime.Now, Player = context.Players.Find(4L)}, //12
                    new GamePlayer{ Game = context.Games.Find(8L), JoinDate = DateTime.Now, Player = context.Players.Find(3L)}, //13
                    new GamePlayer{ Game = context.Games.Find(8L), JoinDate = DateTime.Now, Player = context.Players.Find(4L)}, //14
                };

                foreach (GamePlayer gp in GamePlayers)
                {
                    context.GamePlayers.Add(gp);
                }
            }
            #endregion

            context.SaveChanges();
        }
    }
}
