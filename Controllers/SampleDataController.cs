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
            var list = _db.Athletes.AsQueryable();

            if (filterdata.Filters.Count > 0)
            {
                list =  FilterInner(filterdata.Filters);
            }


            if (!String.IsNullOrEmpty(filterdata.ColId))
            {
                var sort = filterdata.Sort.ToLower() == "desc" ? "desc" : "asc";
                list = list.OrderBy(filterdata.ColId + " " + sort).Take(filterdata.EndRow).Skip(filterdata.StarRow).AsQueryable();
            }
            else
            {
                list = list.Take(filterdata.EndRow).Skip(filterdata.StarRow).AsQueryable();
            }

            return new ResultData
            {
                Items = list.ToList(),
                TotalCount = _db.Athletes.Count()
            };
        }

        private IQueryable<AthleteItem> FilterInner(List<FilterData> filters)
        {
            string expressionFiler = "";
            int cntFilter = 0;
            foreach (var filter in filters)
            {
                expressionFiler += GetComparer(filter.type, filter.filterField, filter.filter);
                if (cntFilter != 0 && cntFilter != filters.Count-1)
                {
                    expressionFiler += " AND ";
                }
                cntFilter++;
            }

            return _db.Athletes.Where(expressionFiler).AsQueryable();
            
        }

        private string GetComparer(string filterType,string field, string value)
        {
            string comparer = "";
            switch (filterType)
            {
                case "equals":
                    comparer = $"{field} = '{value}'";
                    break;
                case "notEqual":
                    comparer = $"{field} != '{value}'";
                    break;
                case "notEquals":
                    comparer = $"{field} != '{value}'";
                    break;
                case "lessThan":
                    comparer = $"{field} < '{value}'";
                    break;
                case "lessThanOrEqual":
                    comparer = $"{field} <= '{value}'";
                    break;
                case "greaterThan":
                    comparer = $"{field} > '{value}'";
                    break;
                case "greaterThanOrEqual":
					comparer = $"{field} >= '{value}'";
                    break;
                case "contains":
                    comparer = $"{field}.Contains(\"{value}\")";
                    break;
                case "startsWith":
                    comparer = $"{field}.StartsWith(\"{value}\")";
                    break;
                case "endsWith":
                    comparer = $"{field}.EndsWith(\"{value}\")";
                    break;
            }
            return comparer;
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

        public List<FilterData> Filters { get; set; }
    }

    public class FilterData
    {
        public string filter { get; set; }
        public string filterType { get; set; }
        public string type { get; set; }
        public string filterField { get; set; }
    }
        
    public class ResultData
    {
        public int TotalCount { get; set; }
        public IEnumerable<AthleteItem> Items { get; set; }
    }
}
