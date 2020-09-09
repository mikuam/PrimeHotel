using System;
using Microsoft.EntityFrameworkCore;

namespace PrimeHotel.Web.Models
{
    [Keyless]
    public class RoomOccupied
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public int RoomNumber { get; set; }

        public int Level { get; set; }

        public bool WithBathroom { get; set; }
    }
}
