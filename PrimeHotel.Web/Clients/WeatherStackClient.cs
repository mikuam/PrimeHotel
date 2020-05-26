using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PrimeHotel.Web.Clients
{
    public class WeatherStackClient : IWeatherStackClient
    {
        private const string ApiKey = "3a1223ae4a4e14277e657f6729cfbdef";
        private const string Username = "Mik";
        private const string Password = "****";

        private const string WeatherStackUrl = "http://api.weatherstack.com/current";

        private HttpClient _client;
        private readonly ILogger<WeatherStackClient> _logger;

        public WeatherStackClient(HttpClient client, ILogger<WeatherStackClient> logger)
        {
            _client = client;
            _logger = logger;

            // authorization is not needed to call WeatherStack.com - this is only to show how to add basic authorization
            var authToken = Encoding.ASCII.GetBytes($"{Username}:{Password}");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(authToken));
        }

        public async Task<WeatherStackResponse> GetCurrentWeather(string city)
        {
            try
            {
                using var responseStream = await _client.GetStreamAsync(GetWeatherStackUrl(city));
                var currentForecast = await JsonSerializer.DeserializeAsync<WeatherStackResponse>(responseStream);

                return currentForecast;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong when calling WeatherStack.com");
                return null;
            }
        }

        public async Task<WeatherStackResponse> GetCurrentWeatherWithCancellationToken(string city)
        {
            try
            {
                using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(3));

                using var responseStream = await _client.GetStreamAsync(GetWeatherStackUrl(city), cancellationTokenSource.Token);
                var currentForecast = await JsonSerializer.DeserializeAsync<WeatherStackResponse>(responseStream);

                return currentForecast;
            }
            catch (TaskCanceledException ec)
            {
                _logger.LogError(ec, $"Call to WeatherStack.com took longer then 3 seconds and had timed out ");
                return null;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong when calling WeatherStack.com");
                return null;
            }
        }

        private string GetWeatherStackUrl(string city)
        {
            return WeatherStackUrl + "?"
                   + "access_key=" + ApiKey
                   + "&query=" + city;
        }
    }
}
