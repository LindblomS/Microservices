using CFS.Domain.Aggregates;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CFS.Infrastructure.Repositories
{
    public class FacilityRepository : IFacilityRepository
    {
        private readonly IDb _db;

        public FacilityRepository(IDb db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<int> Add(Facility facility)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO Facilities (customerId, facilityName, street, city, state, country, zipCode)");
            sql.AppendLine(string.Format("VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')", 
                facility.CustomerId,
                facility.FacilityName,
                facility.Address.Street,
                facility.Address.City,
                facility.Address.State,
                facility.Address.Country,
                facility.Address.ZipCode));

            return await _db.ExecuteAsync(sql.ToString(), null, facility);
        }

        public async Task<int> Update(Facility facility)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE Facilities SET");
            sql.AppendLine($"customerId = '{facility.CustomerId}',");
            sql.AppendLine($"facilityName = '{facility.FacilityName}',");
            sql.AppendLine($"street = '{facility.Address.Street}',");
            sql.AppendLine($"city = '{facility.Address.City}',");
            sql.AppendLine($"state = '{facility.Address.State}',");
            sql.AppendLine($"country = '{facility.Address.Country}',");
            sql.AppendLine($"zipCode = '{facility.Address.ZipCode}'");

            sql.AppendLine($"WHERE facilityId = {facility.Id}");

            return await _db.ExecuteAsync(sql.ToString(), null, facility);
        }

        public async Task<Facility> GetFacility(int id)
        {
            var sql = $"SELECT * FROM Facilities WHERE facilityId = @id";
            return await _db.GetAsync<Facility>(sql, new { id }); ;
        }

        public async Task<int> Delete(Facility facility)
        {
            string sql = $"DELETE FROM Facilities WHERE facilityId = @id";
            return await _db.ExecuteAsync(sql, new { facility.Id }, facility);
        }
    }
}
