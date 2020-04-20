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
            return await primeDbContext.Rooms.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var reminder = await primeDbContext.Rooms.FindAsync(id);
            if (reminder == null)
            {
                return NotFound();
            }

            return Ok(reminder);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Room room)
        {
            var createdRoom = await primeDbContext.Rooms.AddAsync(room);
            await primeDbContext.SaveChangesAsync();

            return Ok(createdRoom.Entity);
        }

        [HttpPut]
        public async Task<IActionResult> Patch([FromBody] Room room)
        {
            var existingRoom = await primeDbContext.Rooms.FindAsync(room.Id);
            if (existingRoom == null)
            {
                return NotFound();
            }

            var updatedRoom = primeDbContext.Update(room);
            return Ok(updatedRoom);
        }
    }
}
