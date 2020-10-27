using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeHotel.Web.Models;

namespace PrimeHotel.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly PrimeDbContext primeDbContext;

        public RoomController(PrimeDbContext _primeDbContext)
        {
            primeDbContext = _primeDbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<Room>> Get()
        {
            return await primeDbContext.Rooms.AsNoTracking().ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var room = await primeDbContext.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Room room)
        {
            var createdRoom = await primeDbContext.Rooms.AddAsync(room);
            await primeDbContext.SaveChangesAsync();

            return Ok(createdRoom.Entity);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Room room)
        {
            var existingRoom = await primeDbContext.Rooms.FindAsync(room.Id);
            if (existingRoom == null)
            {
                return NotFound();
            }

            existingRoom.Number = room.Number;
            existingRoom.Description = room.Description;
            existingRoom.LastBooked = room.LastBooked;
            existingRoom.Level = room.Level;
            existingRoom.RoomType = room.RoomType;
            existingRoom.NumberOfPlacesToSleep = room.NumberOfPlacesToSleep;

            var updatedRoom = primeDbContext.Update(existingRoom);
            await primeDbContext.SaveChangesAsync();
            return Ok(updatedRoom.Entity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingRoom = await primeDbContext.Rooms.FindAsync(id);
            if (existingRoom == null)
            {
                return NotFound();
            }

            var removedRoom = primeDbContext.Rooms.Remove(existingRoom);
            await primeDbContext.SaveChangesAsync();

            return Ok(removedRoom.Entity);
        }
    }
}
