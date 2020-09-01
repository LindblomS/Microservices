using CFS.Domain.SeedWork;
using System.Threading.Tasks;

namespace CFS.Domain.Aggregates
{
    public interface IServiceRepository
    {
        Task<int> Add(Service service);
        Task<int> Update(Service service);
        Task<int> Delete(Service service);
        Task<Service> GetService(int serviceId);
    }
}
