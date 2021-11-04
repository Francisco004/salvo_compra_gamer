﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Models
{
    public class PlayerDTO
    {
        [Key]
        public long Id { get; set; }

        public string Email { get; set; }
    }
}