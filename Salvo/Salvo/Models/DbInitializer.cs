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

            #region Ships
            if (!context.Ships.Any())
            {
                GamePlayer gamePlayer1 = context.GamePlayers.Find(1L);
                GamePlayer gamePlayer2 = context.GamePlayers.Find(2L);
                GamePlayer gamePlayer3 = context.GamePlayers.Find(3L);
                GamePlayer gamePlayer4 = context.GamePlayers.Find(4L);
                GamePlayer gamePlayer5 = context.GamePlayers.Find(5L);
                GamePlayer gamePlayer6 = context.GamePlayers.Find(6L);
                GamePlayer gamePlayer7 = context.GamePlayers.Find(7L);
                GamePlayer gamePlayer8 = context.GamePlayers.Find(8L);
                GamePlayer gamePlayer9 = context.GamePlayers.Find(9L);
                GamePlayer gamePlayer10 = context.GamePlayers.Find(10L);
                GamePlayer gamePlayer11 = context.GamePlayers.Find(11L);
                GamePlayer gamePlayer12 = context.GamePlayers.Find(12L);
                GamePlayer gamePlayer13 = context.GamePlayers.Find(13L);

                var ships = new Ship[]
                {
                    //esta es solo la primera linea de los datos del pdf
                    new Ship
                    {
                        Type = "Destroyer",
                        GamePlayer = gamePlayer1,
                        Locations = new ShipLocation[]
                        {
                            new ShipLocation{Location = "H2"},
                            new ShipLocation{Location = "H3"},
                            new ShipLocation{Location = "H4"}
                        }
                    },
                    new Ship{Type = "Submarine", GamePlayer = gamePlayer1, Locations = new ShipLocation[] {
                            new ShipLocation { Location = "E1" },
                            new ShipLocation { Location = "F1" },
                            new ShipLocation { Location = "G1" }
                        }
                    },
                    new Ship{Type = "PatroalBoat", GamePlayer = gamePlayer1, Locations = new ShipLocation[] {
                            new ShipLocation { Location = "B4" },
                            new ShipLocation { Location = "B5" }
                        }
                    },

                    //obrian gp2
                    new Ship{Type = "Destroyer", GamePlayer = gamePlayer2, Locations = new ShipLocation[] {
                            new ShipLocation { Location = "B5" },
                            new ShipLocation { Location = "C5" },
                            new ShipLocation { Location = "D5" }
                        }
                    },
                    new Ship{Type = "PatroalBoat", GamePlayer = gamePlayer2, Locations = new ShipLocation[] {
                            new ShipLocation { Location = "F1" },
                            new ShipLocation { Location = "F2" }
                        }
                    },

                    //jbauer gp3
                    new Ship{Type = "Destroyer", GamePlayer = gamePlayer3, Locations = new ShipLocation[] {
                            new ShipLocation { Location = "B5" },
                            new ShipLocation { Location = "C5" },
                            new ShipLocation { Location = "D5" }
                        }
                    },
                    new Ship{Type = "PatroalBoat", GamePlayer = gamePlayer3, Locations = new ShipLocation[] {
                            new ShipLocation { Location = "C6" },
                            new ShipLocation { Location = "C7" }
                        }
                    },

                    //obrian gp4
                    new Ship{Type = "Submarine", GamePlayer = gamePlayer4, Locations = new ShipLocation[] {
                            new ShipLocation { Location = "A2" },
                            new ShipLocation { Location = "A3" },
                            new ShipLocation { Location = "A4" }
                        }
                    },

                    new Ship{Type = "PatroalBoat", GamePlayer = gamePlayer4, Locations = new ShipLocation[] {
                            new ShipLocation { Location = "G6" },
                            new ShipLocation { Location = "H6" }
                        }
                    },

                    //obrian gp5
                    new Ship{Type = "Destroyer", GamePlayer = gamePlayer5, Locations = new ShipLocation[] {
                            new ShipLocation { Location = "B5" },
                            new ShipLocation { Location = "C5" },
                            new ShipLocation { Location = "D5" }
                        }
                    },

                    new Ship{Type = "PatroalBoat", GamePlayer = gamePlayer5, Locations = new ShipLocation[] {
                            new ShipLocation { Location = "C6" },
                            new ShipLocation { Location = "C7" }
                        }
                    },

                    //talmeida gp6
                    new Ship{Type = "Submarine", GamePlayer = gamePlayer6, Locations = new ShipLocation[] {
                            new ShipLocation { Location = "A2" },
                            new ShipLocation { Location = "A3" },
                            new ShipLocation { Location = "A4" }
                        }
                    },
                    new Ship{Type = "PatroalBoat", GamePlayer = gamePlayer6, Locations = new ShipLocation[] {
                            new ShipLocation { Location = "G6" },
                            new ShipLocation { Location = "H6" }
                        }
                    },

                    //obrian gp7
                    new Ship{Type = "Destroyer", GamePlayer = gamePlayer7, Locations = new ShipLocation[] {
                            new ShipLocation { Location = "B5" },
                            new ShipLocation { Location = "C5" },
                            new ShipLocation { Location = "D5" }
                        }
                    },
                    new Ship{Type = "PatroalBoat", GamePlayer = gamePlayer7, Locations = new ShipLocation[] {
                            new ShipLocation { Location = "C6" },
                            new ShipLocation { Location = "C7" }
                        }
                    },

                    //jbauer gp8
                    new Ship{Type = "Submarine", GamePlayer = gamePlayer8, Locations = new ShipLocation[] {
                            new ShipLocation { Location = "A2" },
                            new ShipLocation { Location = "A3" },
                            new ShipLocation { Location = "A4" }
                        }
                    },
                    new Ship{Type = "PatroalBoat", GamePlayer = gamePlayer8, Locations = new ShipLocation[] {
                            new ShipLocation { Location = "G6" },
                            new ShipLocation { Location = "H6" }
                        }
                    },

                    //talmeida gp9
                    new Ship{Type = "Destroyer", GamePlayer = gamePlayer9, Locations = new ShipLocation[] {
                            new ShipLocation { Location = "B5" },
                            new ShipLocation { Location = "C5" },
                            new ShipLocation { Location = "D5" }
                        }
                    },
                    new Ship{Type = "PatroalBoat", GamePlayer = gamePlayer9, Locations = new ShipLocation[] {
                            new ShipLocation { Location = "C6" },
                            new ShipLocation { Location = "C7" }
                        }
                    },

                    //jbauer gp10
                    new Ship{Type = "Submarine", GamePlayer = gamePlayer10, Locations = new ShipLocation[] {
                            new ShipLocation { Location = "A2" },
                            new ShipLocation { Location = "A3" },
                            new ShipLocation { Location = "A4" }
                        }
                    },
                    new Ship{Type = "PatroalBoat", GamePlayer = gamePlayer10, Locations = new ShipLocation[] {
                            new ShipLocation { Location = "G6" },
                            new ShipLocation { Location = "H6" }
                        }
                    },

                    //kbauer gp11
                    new Ship{Type = "Destroyer", GamePlayer = gamePlayer11, Locations = new ShipLocation[] {
                            new ShipLocation { Location = "B5" },
                            new ShipLocation { Location = "C5" },
                            new ShipLocation { Location = "D5" }
                        }
                    },
                    new Ship{Type = "PatroalBoat", GamePlayer = gamePlayer11, Locations = new ShipLocation[] {
                            new ShipLocation { Location = "C6" },
                            new ShipLocation { Location = "C7" }
                        }
                    },

                    //kbauer gp12
                    new Ship{Type = "Destroyer", GamePlayer = gamePlayer12, Locations = new ShipLocation[] {
                            new ShipLocation { Location = "B5" },
                            new ShipLocation { Location = "C5" },
                            new ShipLocation { Location = "D5" }
                        }
                    },
                    new Ship{Type = "PatroalBoat", GamePlayer = gamePlayer12, Locations = new ShipLocation[] {
                            new ShipLocation { Location = "C6" },
                            new ShipLocation { Location = "C7" }
                        }
                    },

                    //talmeida gp13
                    new Ship{Type = "Submarine", GamePlayer = gamePlayer13, Locations = new ShipLocation[] {
                            new ShipLocation { Location = "A2" },
                            new ShipLocation { Location = "A3" },
                            new ShipLocation { Location = "A4" }
                        }
                    },
                    new Ship{Type = "PatroalBoat", GamePlayer = gamePlayer13, Locations = new ShipLocation[] {
                            new ShipLocation { Location = "G6" },
                            new ShipLocation { Location = "H6" }
                        }
                    },

                };

                foreach (Ship ship in ships)
                {
                    context.Ships.Add(ship);
                }
            }
            #endregion

            context.SaveChanges();
        }
    }
}
