using CFS.Domain.SeedWork;
using System.Threading.Tasks;

namespace CFS.Domain.Aggregates.ServiceAggregate
{
    public interface IServiceRepository : IRepository<Service>
    {
        Task Add(Service service);
        Task Update(Service service);
        Task<Service> GetService(int serviceId);
    }
}
