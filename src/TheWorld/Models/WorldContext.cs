using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public class WorldContext : DbContext   
    {

        public WorldContext()
        {
            Database.EnsureCreated();
        }
        public DbSet<Trip> Trips { get; set; }

        public DbSet<Stop> Stops { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connString = Startup.Configuration["Data:WorldContetConnection"];

            optionsBuilder.UseSqlServer(connString);

            base.OnConfiguring(optionsBuilder);
        }


    }
}
