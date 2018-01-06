using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static aureliadotnetcore.Controllers.SampleDataController;

namespace aureliadotnetcore.Model
{
    public class MoviesData: DbContext
    {
        public MoviesData(DbContextOptions<MoviesData> options):base(options)
        {

        }
        public DbSet<MovieItem> Movies { get; set; }
        public DbSet<AthleteItem> Athletes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieItem>().ToTable("Movie");
            modelBuilder.Entity<AthleteItem>().ToTable("Athlete");
        }
    }
}
