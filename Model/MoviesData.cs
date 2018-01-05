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
        public MoviesData()
        {

        }

        public MoviesData(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<MovieItem> Movies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Server=(local); Database = movies; integrated security = True";
            optionsBuilder.UseSqlServer(connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        IConfiguration _configuration;
    }
}
