using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeHotel.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeHotel.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly PrimeDbContext primeDbContext;

        public ReservationsController(PrimeDbContext _primeDbContext)
        {
            primeDbContext = _primeDbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<Reservation>> Get()
        {
            return await primeDbContext.Reservations.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var reservation = await primeDbContext.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            await primeDbContext.Entry(reservation).Collection(r => r.ReservationProfiles).LoadAsync();
            await primeDbContext.Entry(reservation).Reference(r => r.Room).LoadAsync();

            return Ok(reservation);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewReservation newReservation)
        {
            var room = await primeDbContext.Rooms.FirstOrDefaultAsync(r => r.Id == newReservation.RoomId);
            var guests = await primeDbContext.Profiles.Where(p => newReservation.GuestIds.Contains(p.Id)).ToListAsync();

            if (room == null || guests.Count != newReservation.GuestIds.Count)
            {
                return NotFound();
            }

            var reservation = new Reservation
            {
                Created = DateTime.UtcNow,
                From = newReservation.From.Value,
                To = newReservation.To.Value,
                Room = room
            };

            var createdReservation = await primeDbContext.Reservations.AddAsync(reservation);
            await primeDbContext.SaveChangesAsync();

            createdReservation.Entity.ReservationProfiles = new List<ReservationProfile>(
                guests.Select(g => new ReservationProfile { ReservationId = createdReservation.Entity.Id, ProfileId = g.Id }));
            await primeDbContext.SaveChangesAsync();

            return Ok(createdReservation.Entity.Id);
        }
    }
}
