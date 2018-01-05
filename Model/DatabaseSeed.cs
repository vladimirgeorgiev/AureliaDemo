using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static aureliadotnetcore.Controllers.SampleDataController;

namespace aureliadotnetcore.Model
{
    public class DatabaseSeed
    {
        MoviesData _db;
        public DatabaseSeed(MoviesData db)
        {
            _db = db;
        }

        public void Seed()
        {
            if (_db.Movies.Count() == 0)
            {
                _db.Movies.AddRange(
                    new MovieItem { Title = "Star Wars", ReleaseYear = 1983 },
                    new MovieItem { Title = "Star Trek", ReleaseYear = 1981 },
                    new MovieItem { Title = "Shrek", ReleaseYear = 2004 }
                );
                _db.SaveChanges();
            }
        }
    }
}
