using CFS.Domain.SeedWork;
using System.Threading.Tasks;

namespace CFS.Domain.Aggregates
{
    public interface IServiceRepository
    {
        Task Add(Service service);
        Task Update(Service service);
        Task Delete(int serviceId);
        Task<Service> GetService(int serviceId);
    }
}
