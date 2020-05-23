using System.Threading.Tasks;

namespace PrimeHotel.Web.Clients
{
    public interface IWeatherStackClient
    {
        Task<WeatherStackResponse> GetCurrentWeather(string city);
    }
}