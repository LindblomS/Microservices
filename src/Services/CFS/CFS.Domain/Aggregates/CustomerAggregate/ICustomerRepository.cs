using CFS.Domain.SeedWork;
using System.Threading.Tasks;

namespace CFS.Domain.Aggregates.CustomerAggregate
{
    public interface ICustomerRepository
    {
        Task Add(Customer customer);
        Task Update(Customer customer);
        Task Delete(int customerId);
        Task<Customer> GetCustomer(int customerId);
    }
}
