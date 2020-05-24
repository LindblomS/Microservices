using CFS.Domain.Aggregates.ServiceAggregate;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CFS.Infrastructure.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly DataContext _context;

        public ServiceRepository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Add(Service service)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO Services (facilityId, startDate, stopDate)");
            sql.AppendLine(string.Format("VALUES ('{0}', '{1}', '{2}')",
                service.FacilityId,
                service.StartDate,
                service.StopDate));

            await _context.ExecuteNonQueryAsync(sql.ToString());
        }

        public async Task Update(Service service)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE Services SET");
            sql.AppendLine($"facilityId = {service.FacilityId}");
            sql.AppendLine($"startDate = '{service.StartDate}'");
            sql.AppendLine($"stopDate = '{service.StopDate}'");

            sql.AppendLine($"WHERE serviceId = {service.Id}");

            await _context.ExecuteNonQueryAsync(sql.ToString());
        }

        public async Task<Service> GetService(int serviceId)
        {
            var sql = $"SELECT * FROM CFS_Services WHERE serviceId = {serviceId}";
            return await _context.QueryAsync<Service>(sql);
        }

        public async Task Delete(int serviceId)
        {
            string sql = $"DELETE FROM Services WHERE serviceId = {serviceId}";
            await _context.ExecuteNonQueryAsync(sql);
        }
    }
}
