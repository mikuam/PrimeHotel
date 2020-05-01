using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PrimeHotel.Web.Models;

namespace PrimeHotel.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly PrimeDbContext primeDbContext;
        private readonly string connectionString;

        public ProfileController(PrimeDbContext _primeDbContext, IConfiguration _configuration)
        {
            connectionString = _configuration.GetConnectionString("HotelDB");
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

        [HttpPost("GenerateAndInsert")]
        public async Task<IActionResult> GenerateAndInsert([FromBody] int count = 1000)
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            var profiles = GenerateProfiles(count);
            var gererationTime = s.Elapsed.ToString();
            s.Restart();

            primeDbContext.Profiles.AddRange(profiles);
            var insertedCount = await primeDbContext.SaveChangesAsync();

            return Ok(new {
                    inserted = insertedCount,
                    generationTime = gererationTime,
                    insertTime = s.Elapsed.ToString()
                });
        }

        [HttpPost("GenerateAndInsertWithSqlCopy")]
        public async Task<IActionResult> GenerateAndInsertWithSqlCopy([FromBody] int count = 1000)
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            var profiles = GenerateProfiles(count);
            var gererationTime = s.Elapsed.ToString();
            s.Restart();

            var dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Ref");
            dt.Columns.Add("Forename");
            dt.Columns.Add("Surname");
            dt.Columns.Add("Email");
            dt.Columns.Add("TelNo");
            dt.Columns.Add("DateOfBirth");

            foreach (var profile in profiles)
            {
                dt.Rows.Add(string.Empty, profile.Ref, profile.Forename, profile.Surname, profile.Email, profile.TelNo, profile.DateOfBirth);
            }

            using var sqlBulk = new SqlBulkCopy(connectionString);
            sqlBulk.DestinationTableName = "Profiles";
            await sqlBulk.WriteToServerAsync(dt);

            return Ok(new
            {
                inserted = dt.Rows.Count,
                generationTime = gererationTime,
                insertTime = s.Elapsed.ToString()
            });
        }

        private IEnumerable<Profile> GenerateProfiles(int count)
        {
            var profileGenerator = new Faker<Profile>()
                .RuleFor(p => p.Ref, v => v.Person.UserName)
                .RuleFor(p => p.Forename, v => v.Person.FirstName)
                .RuleFor(p => p.Surname, v => v.Person.LastName)
                .RuleFor(p => p.Email, v => v.Person.Email)
                .RuleFor(p => p.TelNo, v => v.Person.Phone)
                .RuleFor(p => p.DateOfBirth, v => v.Person.DateOfBirth);

            return profileGenerator.Generate(count);
        }
    }
}
