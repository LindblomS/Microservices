using CFS.Domain.SeedWork;
using System.Threading.Tasks;

namespace CFS.Domain.Aggregates.CustomerAggregate
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        void Add(Customer customer);
        void Update(Customer customer);
        Customer GetCustomer(int customerId);
    }
}
