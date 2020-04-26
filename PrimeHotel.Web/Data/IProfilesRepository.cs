using System.Collections.Generic;
using System.Threading.Tasks;
using PrimeHotel.Web.Models;

namespace PrimeHotel.Web.Data
{
    public interface IProfilesRepository
    {
        Task<IEnumerable<Profile>> GetAllAsync();

        Task<Profile> GetAsync(int profileId);

        Task AddAsync(Profile newProfile);

        void Update(Profile profile);

        void Remove(int profileId);
    }
}