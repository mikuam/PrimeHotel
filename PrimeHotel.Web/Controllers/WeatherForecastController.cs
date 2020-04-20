using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PrimeHotel.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        // GET: weatherForecast?sortByTemperature=true
        [HttpGet]
        public IEnumerable<WeatherForecast> Get([FromQuery]bool sortByTemperature = false)
        {
            var rng = new Random();
            var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });

            if (sortByTemperature)
            {
                forecasts = forecasts.OrderByDescending(f => f.TemperatureC);
            }

            return forecasts;
        }

        // GET: weatherForecast/3
        [Route("{daysForward}")]
        [HttpGet]
        public IActionResult Get(int daysForward)
        {
            if (daysForward == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            var rng = new Random();
            return new JsonResult(new WeatherForecast
            {
                Date = DateTime.Now.AddDays(daysForward),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }

        // POST: weatherForecast/
        [HttpPost]
        public IActionResult Post([FromBody] WeatherForecast forecast, [FromHeader] string parentRequestId)
        {
            try
            {
                Console.WriteLine($"Got a forecast for data: {forecast.Date} with parentRequestId: {parentRequestId}!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return new AcceptedResult();
        }

        // POST: weatherForecast/sendfile
        [Route("sendfile")]
        [HttpPost]
        public IActionResult SaveFile([FromForm] string fileName, [FromForm] IFormFile file)
        {
            Console.WriteLine($"Got a file with name: {fileName} and size: {file.Length}");
            return new AcceptedResult();
        }
    }
}
