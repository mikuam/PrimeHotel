using System;

namespace PrimeHotel.Web.Models
{
    public class Room
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public string Description { get; set; }

        public DateTime LastBooked { get; set; }

        public int Level { get; set; }

        public RoomType RoomType { get; set; }

        public bool WithBathroom { get; set; }

        public int NumberOfPlacesToSleep { get; set; }
    }

    public enum RoomType
    {
        Standard,
        Suite
    }
}
