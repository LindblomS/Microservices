using CFS.Domain.SeedWork;
using System.Threading.Tasks;

namespace CFS.Domain.Aggregates.ServiceAggregate
{
    public interface IServiceRepository : IRepository<Service>
    {
        void Add(Service service);
        void Update(Service service);
        Service GetService(int serviceId);
    }
}
