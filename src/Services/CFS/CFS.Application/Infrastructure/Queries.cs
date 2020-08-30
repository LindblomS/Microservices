using CFS.Application.Application.Queries;
using CFS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Infrastructure
{
    public class Queries : IQueries
    {
        private readonly IDbQueries _db;

        public Queries(IDbQueries db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<CustomerViewModel> GetCustomer(int id)
        {
            var sql = "SELECT * FROM Customers WHERE customerId = @id";
            return await _db.GetAsync<CustomerViewModel>(sql, id);
        }

        public async Task<IList<CustomerViewModel>> GetCustomers()
        {
            var sql = "SELECT * FROM Customers";
            return await _db.SelectAsync<CustomerViewModel>(sql, null);
        }

        public async Task<IList<FacilityViewModel>> GetFacilities()
        {
            var sql = "SELECT * FROM Facilities";
            return await _db.SelectAsync<FacilityViewModel>(sql, null);
        }

        public async Task<IList<FacilityViewModel>> GetFacilitiesOnCustomer(int id)
        {
            var sql = "SELECT * FROM Facilities WHERE customerId = @id";
            return await _db.SelectAsync<FacilityViewModel>(sql, id);
        }

        public async Task<FacilityViewModel> GetFacility(int id)
        {
            var sql = "SELECT * FROM Facilities WHERE facilityId = @id";
            return await _db.GetAsync<FacilityViewModel>(sql, id);
        }

        public async Task<ServiceViewModel> GetService(int id)
        {
            var sql = "SELECT * FROM Services WHERE serviceId = @id";
            return await _db.GetAsync<ServiceViewModel>(sql, id);
        }

        public async Task<IList<ServiceViewModel>> GetServices()
        {
            var sql = $"SELECT * FROM Services";
            return await _db.SelectAsync<ServiceViewModel>(sql, null);
        }

        public async Task<IList<ServiceViewModel>> GetServicesOnCustomer(int id)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT * FROM Services s");
            sql.AppendLine("JOIN Facilities f ON f.facilityId = s.facilityId");
            sql.AppendLine("WHERE f.customerId = @id");

            return await _db.SelectAsync<ServiceViewModel>(sql.ToString(), id);
        }
    }
}
