using CFS.Domain.Aggregates;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CFS.Infrastructure.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly IDbWithTransaction _db;

        public ServiceRepository(IDbWithTransaction db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task Add(Service service)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO Services (facilityId, startDate, stopDate)");
            sql.AppendLine(string.Format("VALUES ('{0}', '{1}', '{2}')",
                service.FacilityId,
                service.StartDate,
                service.StopDate));

            await _db.ExecuteAsync(sql.ToString(), null);
        }

        public async Task Update(Service service)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE Services SET");
            sql.AppendLine($"facilityId = {service.FacilityId},");
            sql.AppendLine($"startDate = '{service.StartDate}',");
            sql.AppendLine($"stopDate = '{service.StopDate}'");

            sql.AppendLine($"WHERE serviceId = {service.Id}");

            await _db.ExecuteAsync(sql.ToString(), null);
        }

        public async Task<Service> GetService(int id)
        {
            var sql = $"SELECT * FROM CFS_Services WHERE serviceId = @id";
            return await _db.GetAsync<Service>(sql, new { id }); ;
        }

        public async Task Delete(int id)
        {
            string sql = $"DELETE FROM Services WHERE serviceId = @id";
            await _db.ExecuteAsync(sql, new { id });
        }
    }
}
