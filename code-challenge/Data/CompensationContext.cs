using challenge.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Data
{
    public class CompensationContext : DbContext  //Dbset for compensation (Task2)
    {
        public CompensationContext(DbContextOptions<CompensationContext> options) : base(options)
        {
            
        }

        public DbSet<Compensation> Compensation { get; set; }
    }
}
