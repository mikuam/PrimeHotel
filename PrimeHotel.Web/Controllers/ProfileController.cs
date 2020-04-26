using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeHotel.Web.Models;

namespace PrimeHotel.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly PrimeDbContext primeDbContext;

        public ProfileController(PrimeDbContext _primeDbContext)
        {
            primeDbContext = _primeDbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<Profile>> Get()
        {
            return await primeDbContext.Profiles.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var profile = await primeDbContext.Profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Profile profile)
        {
            var createdProfile = await primeDbContext.Profiles.AddAsync(profile);
            await primeDbContext.SaveChangesAsync();

            return Ok(createdProfile.Entity);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Profile profile)
        {
            var existingProfile = await primeDbContext.Profiles.FindAsync(profile.Id);
            if (existingProfile == null)
            {
                return NotFound();
            }

            existingProfile.Ref = profile.Ref;
            existingProfile.Forename = profile.Forename;
            existingProfile.Surname = profile.Surname;
            existingProfile.Email = profile.Email;
            existingProfile.DateOfBirth = profile.DateOfBirth;
            existingProfile.TelNo = profile.TelNo;

            var updatedProfile = primeDbContext.Update(existingProfile);
            await primeDbContext.SaveChangesAsync();
            return Ok(updatedProfile.Entity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingProfile = await primeDbContext.Profiles.FindAsync(id);
            if (existingProfile == null)
            {
                return NotFound();
            }

            var removedProfile = primeDbContext.Profiles.Remove(existingProfile);
            await primeDbContext.SaveChangesAsync();

            return Ok(removedProfile.Entity);
        }
    }
}
