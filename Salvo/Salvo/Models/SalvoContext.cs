using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Models
{
    public class SalvoContext : DbContext 
    {
        public SalvoContext(DbContextOptions<SalvoContext>options) : base(options)
        {

        }

        //public DbSet<demo> demos { set; get; }
    }

    /*
    public class demo
    {
        public int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
    }
    */

}
