using CFS.Domain.SeedWork;
using System.Threading.Tasks;

namespace CFS.Domain.Aggregates.CustomerAggregate
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task Add(Customer customer);
        Task Update(Customer customer);
        Task<Customer> GetCustomer(int customerId);
    }
}
