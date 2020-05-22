using CFS.Domain.Aggregates.FacilityAggregate;
using CFS.Domain.SeedWork;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CFS.Infrastructure.Repositories
{
    public class FacilityRepository : IFacilityRepository
    {
        private readonly DataContext _context;

        public FacilityRepository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Add(Facility facility)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO CFS.Facilities (customerId, street, city, state, country, zipCode)");
            sql.AppendLine(string.Format("VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", 
                facility.CustomerId,
                facility.Address.Street,
                facility.Address.City,
                facility.Address.State,
                facility.Address.Country,
                facility.Address.ZipCode));

            await _context.ExecuteAsync(sql.ToString());
        }

        public async Task Update(Facility facility)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE CFS.Facilities SET");
            sql.AppendLine($"customerId = '{facility.CustomerId}'");
            sql.AppendLine($"street = '{facility.Address.Street}'");
            sql.AppendLine($"city = '{facility.Address.City}'");
            sql.AppendLine($"state = '{facility.Address.State}'");
            sql.AppendLine($"country = '{facility.Address.Country}'");
            sql.AppendLine($"zipCode = '{facility.Address.ZipCode}'");

            sql.AppendLine($"WHERE facilityId = {facility.Id}");

            await _context.ExecuteAsync(sql.ToString());
        }

        public async Task<Facility> GetFacility(int facilityId)
        {
            var sql = $"SELECT * FROM CFS.Facilities WHERE facilityId = {facilityId}";
            return await _context.QueryAsync<Facility>(sql);
        }

        public async Task Delete(int facilityId)
        {
            string sql = $"DELETE FROM CFS.Facilities WHERE facilityId = {facilityId}";
            await _context.ExecuteAsync(sql);
        }
    }
}
