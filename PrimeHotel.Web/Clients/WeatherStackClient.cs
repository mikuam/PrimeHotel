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
        }

        public async Task<WeatherStackResponse> GetCurrentWeather(string city)
        {
            try
            {
                var response = await _client.GetAsync(GetWeatherStackUrl(city));
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var currentForecast = await JsonSerializer.DeserializeAsync<WeatherStackResponse>(responseStream);

                return currentForecast;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong when calling WeatherStack.com");
                return null;
            }
        }

        public async Task<WeatherStackResponse> GetCurrentWeatherWithAuth(string city)
        {
            try
            {
                using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(3));

                var authToken = Encoding.ASCII.GetBytes($"{Username}:{Password}");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(authToken));

                var response = await _client.GetAsync(GetWeatherStackUrl(city), cancellationTokenSource.Token);
                using var responseStream = await response.Content.ReadAsStreamAsync();
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
