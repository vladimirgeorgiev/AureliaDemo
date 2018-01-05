using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using aureliadotnetcore.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aureliadotnetcore.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        MoviesData _db;

        public SampleDataController(MoviesData db)
        {
            _db = db;
        }

        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }

        [HttpGet("[action]")]
        public IEnumerable<MovieItem> Movies() {

            //Thread.Sleep(1000);
            return _db.Movies.ToList();
        }
        [HttpGet("[action]/{id}")]
        public MovieItem Movies(int id)
        {
            //Thread.Sleep(1000);
            var movie = _db.Movies.SingleOrDefault(x => x.Id == id);
            return movie;
        }
        [HttpPut("[action]")]
        public MovieItem Movies([FromBody]MovieItem updatedMovie)
        {
            if (ModelState.IsValid)
            {
                if (updatedMovie.Id > 0)
                {
                    _db.Movies.Attach(updatedMovie).State = EntityState.Modified;
                    _db.SaveChanges();
                }
                else
                {
                    _db.Movies.Add(updatedMovie).State = EntityState.Added;
                    _db.SaveChanges();
                }
                
                return updatedMovie;
            }
            return null;
        }
     
        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(this.TemperatureC / 0.5556);
                }
            }
        }

        public class MovieItem
        {
            public string Title { get; set; }
            public int ReleaseYear { get; set; }
            public int Id { get; set; }
        }
    }
}
