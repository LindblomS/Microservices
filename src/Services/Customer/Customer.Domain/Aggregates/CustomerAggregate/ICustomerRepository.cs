using CFS.Domain.SeedWork;
using System.Threading.Tasks;

namespace CFS.Domain.Aggregates.CustomerAggregate
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Customer Add(Customer customer);
        void Update(Customer customer);
        Task<Customer> GetCustomerAsync(int customerId);
    }
}
