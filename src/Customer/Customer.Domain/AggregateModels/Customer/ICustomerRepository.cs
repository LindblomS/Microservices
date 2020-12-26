namespace Services.Customer.Domain
{
    using System;
    using System.Threading.Tasks;
    using Services.Customer.Domain.SeedWork;

    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> GetAsync(Guid customerId);
        Task<Customer> CreateAsync(Customer customer);
        Task DeleteAsync(Guid customerId);
        Task UpdateAsync(Customer customer);
    }
}
