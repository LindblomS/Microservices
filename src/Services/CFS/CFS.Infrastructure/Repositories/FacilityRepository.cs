﻿using CFS.Domain.Aggregates.FacilityAggregate;
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
            sql.AppendLine("INSERT INTO Facilities (customerId, facilityName, street, city, state, country, zipCode)");
            sql.AppendLine(string.Format("VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')", 
                facility.CustomerId,
                facility.FacilityName,
                facility.Address.Street,
                facility.Address.City,
                facility.Address.State,
                facility.Address.Country,
                facility.Address.ZipCode));

            await _context.ExecuteNonQueryAsync(sql.ToString());
        }

        public async Task Update(Facility facility)
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

            await _context.ExecuteNonQueryAsync(sql.ToString());
        }

        public async Task<Facility> GetFacility(int facilityId)
        {
            var sql = $"SELECT * FROM Facilities WHERE facilityId = {facilityId}";
            return await _context.QueryAsync<Facility>(sql);
        }

        public async Task Delete(int facilityId)
        {
            string sql = $"DELETE FROM Facilities WHERE facilityId = {facilityId}";
            await _context.ExecuteNonQueryAsync(sql);
        }
    }
}
