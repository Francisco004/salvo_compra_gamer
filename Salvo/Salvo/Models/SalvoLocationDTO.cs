using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Models
{
    public class SalvoLocationDTO
    {
        [Key]
        public long Id { get; set; }
        public string Location { get; set; }
    }
}
