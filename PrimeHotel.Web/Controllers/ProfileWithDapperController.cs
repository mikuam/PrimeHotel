using Microsoft.AspNetCore.Mvc;
using PrimeHotel.Web.Data;
using PrimeHotel.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrimeHotel.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfileWithDapperController : ControllerBase
    {
        private readonly IProfilesRepository profilesRepository;

        public ProfileWithDapperController(IProfilesRepository _profilesRepository)
        {
            profilesRepository = _profilesRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Profile>> Get()
        {
            return await profilesRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var profile = await profilesRepository.GetAsync(id);
            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Profile profile)
        {
            await profilesRepository.AddAsync(profile);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Profile profile)
        {
            var existingProfile = await profilesRepository.GetAsync(profile.Id);
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

            profilesRepository.Update(existingProfile);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingProfile = await profilesRepository.GetAsync(id);
            if (existingProfile == null)
            {
                return NotFound();
            }

            profilesRepository.Remove(id);

            return Ok();
        }
    }
}
