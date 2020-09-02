using Microsoft.EntityFrameworkCore;

namespace PrimeHotel.Web.Models
{
    public class PrimeDbContext : DbContext
    {
        public PrimeDbContext(DbContextOptions<PrimeDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Room> Rooms { get; set; }

        public virtual DbSet<Profile> Profiles { get; set; }

        public virtual DbSet<Reservation> Reservations { get; set; }

        // from stored procedures
        public virtual DbSet<GuestArrival> GuestArrivals { get; set; }
    }
}
