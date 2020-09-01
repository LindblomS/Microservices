using CFS.Domain.SeedWork;
using System.Threading.Tasks;

namespace CFS.Domain.Aggregates
{
    public interface ICustomerRepository
    {
        Task<int> Add(Customer customer);
        Task<int> Update(Customer customer);
        Task<int> Delete(Customer customer);
        Task<Customer> GetCustomer(int customerId);
    }
}
