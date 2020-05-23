using System.Text.Json.Serialization;

namespace PrimeHotel.Web.Clients
{
    public class WeatherStackResponse
    {
        [JsonPropertyName("current")]
        public Current CurrentWeather { get; set; }

        public class Current
        {
            [JsonPropertyName("temperature")]
            public int Temperature { get; set; }

            [JsonPropertyName("weather_descriptions")]
            public string[] WeatherDescriptions { get; set; }

            [JsonPropertyName("wind_speed")]
            public int WindSpeed { get; set; }

            [JsonPropertyName("pressure")]
            public int Pressure { get; set; }

            [JsonPropertyName("humidity")]
            public int Humidity { get; set; }

            [JsonPropertyName("feelslike")]
            public int FeelsLike { get; set; }
        }
    }
}
