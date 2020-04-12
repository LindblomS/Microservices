using CFS.Domain.SeedWork;
using System.Threading.Tasks;

namespace CFS.Domain.Aggregates.ServiceAggregate
{
    public interface IServiceRepository : IRepository<Service>
    {
        Service Add(Service service);
        void Update(Service service);
        Task<Service> GetCustomerAsync(int serviceId);
    }
}
