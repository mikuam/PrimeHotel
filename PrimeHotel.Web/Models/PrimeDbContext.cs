using System.Security.Cryptography.X509Certificates;
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
    }
}
