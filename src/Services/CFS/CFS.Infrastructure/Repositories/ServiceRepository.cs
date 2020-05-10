using CFS.Domain.Aggregates.ServiceAggregate;
using CFS.Domain.SeedWork;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CFS.Infrastructure.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly DataContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public ServiceRepository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Add(Service service)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO CFS.Services (facilityId, startDate, stopDate)");
            sql.AppendLine(string.Format("VALUES ('{0}', '{1}', '{2}')",
                service.FacilityId,
                service.StartDate,
                service.StopDate));

            await _context.ExecuteAsync(sql.ToString());
        }

        public async Task Update(Service service)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE CFS.Services SET");
            sql.AppendLine($"facilityId = {service.FacilityId}");
            sql.AppendLine($"startDate = '{service.StartDate}'");
            sql.AppendLine($"stopDate = '{service.StopDate}'");

            sql.AppendLine($"WHERE serviceId = {service.Id}");

            await _context.ExecuteAsync(sql.ToString());
        }

        public async Task<Service> GetService(int serviceId)
        {
            var sql = $"SELECT * FROM CFS.Services WHERE serviceId = {serviceId}";
            return await _context.QueryAsync<Service>(sql);
        }
    }
}
