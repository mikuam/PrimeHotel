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

        public virtual DbSet<Address> Address { get; set; }

        // from stored procedures
        public virtual DbSet<GuestArrival> GuestArrivals { get; set; }

        // from views
        public virtual DbSet<RoomOccupied> RoomsOccupied { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<ReservationProfile>()
                .HasKey(t => new {t.ReservationId, t.ProfileId});

            modelBuilder
                .Entity<ReservationProfile>()
                .HasOne(r => r.Reservation)
                .WithMany(p => p.ReservationProfiles)
                .HasForeignKey(rp => rp.ReservationId);

            modelBuilder
                .Entity<ReservationProfile>()
                .HasOne(r => r.Profile)
                .WithMany(p => p.ReservationProfiles)
                .HasForeignKey(rp => rp.ProfileId);

            modelBuilder
                .Entity<RoomOccupied>(eb =>
                {
                    eb.HasNoKey();
                    eb.ToView("vwRoomsOccupied");
                });
        }
    }
}
