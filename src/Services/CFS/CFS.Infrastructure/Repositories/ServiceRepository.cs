using CFS.Domain.Aggregates.ServiceAggregate;
using CFS.Domain.SeedWork;
using System;
using System.Text;

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

        public void Add(Service service)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO CFS.Services (startDate, stopDate)");
            sql.AppendLine(string.Format("VALUES ('{0}', '{1}')",
                service.StartDate,
                service.StopDate));

            _context.Execute(sql.ToString());
        }

        public void Update(Service service)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE CFS.Services SET");
            sql.AppendLine($"startDate = '{service.StartDate}'");
            sql.AppendLine($"stopDate = '{service.StopDate}'");

            sql.AppendLine($"WHERE serviceId = {service.Id}");

            _context.Execute(sql.ToString());
        }

        public Service GetService(int serviceId)
        {
            var sql = $"SELECT * FROM CFS.Services WHERE serviceId = {serviceId}";
            return _context.Query<Service>(sql);
        }
    }
}
