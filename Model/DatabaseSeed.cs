using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace aureliadotnetcore.Model
{
    public class DatabaseSeed
    {
        MoviesData _db;
        IConfiguration _configuration;

        public DatabaseSeed(MoviesData db)
        {
            _db = db;
        }

        public async Task Seed(IConfiguration configuration)
        {
            _configuration = configuration;

            if (_db.Movies.Count() == 0)
            {
                _db.Movies.AddRange(
                    new MovieItem { Title = "Star Wars", ReleaseYear = 1983 },
                    new MovieItem { Title = "Star Trek", ReleaseYear = 1981 },
                    new MovieItem { Title = "Shrek", ReleaseYear = 2004 }
                );
                _db.SaveChanges();
            }

            var urlToJson = configuration.GetConnectionString("jsonPathToInitialData");
            if (_db.Athletes.Count() == 0)
            {
                using (var httpClient = new HttpClient())
                {
                    var json = await httpClient.GetStringAsync(urlToJson);
                    var items = JsonConvert.DeserializeObject<List<AthleteItem>>(json, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    _db.Athletes.AddRange(items);
                    await _db.SaveChangesAsync();
                }
            }

        }
    }
}
