using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using PrimeHotel.Web.Models;
using Dapper;

namespace PrimeHotel.Web.Data
{
    public class ProfilesRepository : IProfilesRepository
    {
        private readonly IConfiguration _configuration;
        private IDbConnection _connection;

        public ProfilesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection(_configuration.GetConnectionString("HotelDB"));
        }

        public async Task<IEnumerable<Profile>> GetAllAsync()
        {
            return await GetOpenedConnection().QueryAsync<Profile>(SQL.GetAll);
        }

        public async Task<Profile> GetAsync(int profileId)
        {
            return await GetOpenedConnection().QueryFirstAsync<Profile>(SQL.GetAll, new { id = profileId });
        }

        public async Task AddAsync(Profile newProfile)
        {
            await GetOpenedConnection().ExecuteAsync(SQL.Add, newProfile);
        }

        public void Update(Profile profile)
        {
            GetOpenedConnection().Execute(SQL.Update, profile);
        }

        public void Remove(int profileId)
        {
            GetOpenedConnection().Execute(SQL.Remove, new { id = profileId });
        }

        private IDbConnection GetOpenedConnection()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }

            return _connection;
        }

        internal static class SQL
        {
            internal const string GetAll = @"
                SELECT *
                FROM Profiles";

            internal const string Get = @"
                SELECT *
                FROM Profiles
	            WHERE Id = @id";

            internal const string Add = @"
                INSERT INTO Profiles (Ref, Forename, Surname, TelNo, Email, DateOfBirth)
                VALUES(@Ref, @Forename, @Surname, @TelNo, @Email, @DateOfBirth)";

            internal const string Update = @"
                UPDATE Profiles
                SET
                    Ref = @Ref,
                    Forename = @Forename,
                    Surname = @Surname,
                    TelNo = @TelNo,
                    Email = @Email,
                    DateOfBirth = @DateOfBirth
                WHERE Id = @id";

            internal const string Remove = @"
                DELETE
                FROM Profiles
                WHERE Id = @Id";
        }
    }

}
