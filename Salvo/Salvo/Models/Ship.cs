using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Models
{
    public class Ship
    {
        [Key]
        public long Id { get; set; }

        public string Type { get; set; }
        
        public long GamePlayerId { get; set; }
        public GamePlayer GamePlayer { get; set; }

        public ICollection<ShipLocation> Locations { get; set; }
    }
}
