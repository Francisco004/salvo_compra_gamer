using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Models
{
    public class SalvoDTO
    {
        [Key]
        public long Id { get; set; }

        public PlayerDTO Player { get; set; }

        public long Turn { get; set; }

        public ICollection<SalvoLocationDTO> Locations { get; set; }
    }
}
