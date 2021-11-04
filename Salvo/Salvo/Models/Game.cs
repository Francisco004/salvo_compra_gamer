using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Models
{
    public class Game
    {
        [Key]
        public long Id { get; set; }

        public DateTime? CreationDate { get; set; }

        public ICollection<GamePlayer> GamePlayers { get; set; }
    }
}
