using System.Text.Json.Serialization;

namespace PrimeHotel.Web.Models
{
    public class ReservationProfile
    {
        public int ReservationId { get; set; }

        [JsonIgnore]
        public Reservation Reservation { get; set; }

        public int ProfileId { get; set; }

        [JsonIgnore]
        public Profile Profile { get; set; }
    }
}
