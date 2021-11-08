using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Models
{
    public class SalvoLocation
    {
        public long Id { get; set; }

        public long salvoID { get; set; }

        public Salvo Salvo { get; set; }

        public string Location { get; set; }
    }
}
