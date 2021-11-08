﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Models
{
    public class ShipDTO
    {
        [Key]
        public long Id { get; set; }

        public string Type { get; set; }

        public ICollection<ShipLocationDTO> Locations { get; set; }
    }
}
