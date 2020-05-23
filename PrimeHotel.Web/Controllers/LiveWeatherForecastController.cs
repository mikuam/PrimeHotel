using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrimeHotel.Web.Clients;

namespace PrimeHotel.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LiveWeatherForecastController : ControllerBase
    {
        private readonly IWeatherStackClient _weatherStackClient;

        public LiveWeatherForecastController(IWeatherStackClient weatherStackClient)
        {
            _weatherStackClient = weatherStackClient;
        }

        // GET: liveWeatherForecast?city={city}
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]string city)
        {
            var response = await _weatherStackClient.GetCurrentWeather(city);

            return Ok(response);
        }
    }
}
