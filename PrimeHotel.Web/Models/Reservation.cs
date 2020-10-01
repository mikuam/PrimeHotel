using System;
using System.Collections.Generic;

namespace PrimeHotel.Web.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public Room Room { get; set; }

        public List<ReservationProfile> ReservationProfiles { get; set; }

        public DateTime Created { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}
