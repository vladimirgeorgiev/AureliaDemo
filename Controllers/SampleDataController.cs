using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using aureliadotnetcore.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace aureliadotnetcore.Controllers
{
    [Route("api/[controller]")]
    public partial class SampleDataController : Controller
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
        public IEnumerable<MovieItem> Movies()
        {

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

        [HttpPost("[action]")]
        public ResultData Athletes([FromBody]SortFilterdata filterdata)
        {
            var list = new List<AthleteItem>();
            if (!String.IsNullOrEmpty(filterdata.ColId))
            {
                var sort = filterdata.Sort.ToLower() == "desc" ? "desc" : "asc";
                list = _db.Athletes.OrderBy(filterdata.ColId + " " + sort).Take(filterdata.EndRow).Skip(filterdata.StarRow).ToList();
            }
            else
            {
                list = _db.Athletes.Take(filterdata.EndRow).Skip(filterdata.StarRow).ToList();
            }
            return new ResultData
            {
                Items = list,
                TotalCount = _db.Athletes.Count()
            };
        }

        [HttpPut("[action]")]
        public AthleteItem Athletes([FromBody]AthleteItem athlete)
        {
            _db.Athletes.Attach(athlete).State = EntityState.Modified;
            _db.SaveChangesAsync();
            return athlete;
        }
    }

    public class SortFilterdata
    {
        public int StarRow { get; set; }
        public int EndRow { get; set; }
        public string ColId { get; set; }
        public string Sort { get; set; }
    }

    public class ResultData
    {
        public int TotalCount { get; set; }
        public IEnumerable<AthleteItem> Items { get; set; }
    }
}
