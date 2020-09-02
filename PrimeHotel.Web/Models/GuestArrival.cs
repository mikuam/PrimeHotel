using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PrimeHotel.Web.Models
{
    [Keyless]
    public class GuestArrival
    {
        public string Forename { get; set; }

        public string Surname { get; set; }

        public string TelNo { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public int RoomNumber { get; set; }
    }
}
