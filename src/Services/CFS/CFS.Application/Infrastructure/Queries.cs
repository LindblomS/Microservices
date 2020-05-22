using CFS.Application.Application.Queries;
using CFS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CFS.Application.Infrastructure
{
    public class Queries : IQueries
    {
        private readonly DataContext _context;

        public Queries(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<CustomerViewModel> GetCustomer(int customerId)
        {
            var sql = $"SELECT * FROM CFS.Customers WHERE customerId = {customerId}";
            return await _context.QueryAsync<CustomerViewModel>(sql);
        }

        public async Task<List<CustomerViewModel>> GetCustomers()
        {
            var sql = $"SELECT * FROM CFS.Customers";
            return await _context.ListQueryAsync<CustomerViewModel>(sql);
        }

        public async Task<List<FacilityViewModel>> GetFacilities()
        {
            var sql = $"SELECT * FROM CFS.Facilities";
            return await _context.ListQueryAsync<FacilityViewModel>(sql);
        }

        public async Task<List<FacilityViewModel>> GetFacilitiesOnCustomer(int customerId)
        {
            var sql = $"SELECT * FROM CFS.Facilities WHERE customerId = {customerId}";
            return await _context.ListQueryAsync<FacilityViewModel>(sql);
        }

        public async Task<FacilityViewModel> GetFacility(int facilityId)
        {
            var sql = $"SELECT * FROM CFS.Facilities WHERE facilityId = {facilityId}";
            return await _context.QueryAsync<FacilityViewModel>(sql);
        }

        public async Task<ServiceViewModel> GetService(int serviceId)
        {
            var sql = $"SELECT * FROM CFS.Services WHERE serviceId = {serviceId}";
            return await _context.QueryAsync<ServiceViewModel>(sql);
        }

        public async Task<List<ServiceViewModel>> GetServices()
        {
            var sql = $"SELECT * FROM CFS.Services";
            return await _context.ListQueryAsync<ServiceViewModel>(sql);
        }

        public async Task<List<ServiceViewModel>> GetServicesOnCustomer(int customerId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT * FROM CFS.Services s");
            sql.AppendLine("JOIN CFS.Facilities f ON f.facilityId = s.facilityId");
            sql.AppendLine($"WHERE f.customerId =  {customerId}");

            return await _context.ListQueryAsync<ServiceViewModel>(sql.ToString());
        }
    }
}
