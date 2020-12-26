namespace Services.Customer.Infrastructure
{
    using Services.Customer.Domain;
    using Services.Customer.Domain.SeedWork;
    using System;
    using System.Threading.Tasks;

    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerContext _context;

        public CustomerRepository(CustomerContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<Customer> CreateAsync(Customer customer)
        {
            return (await _context.AddAsync(customer)).Entity;
        }

        public async Task DeleteAsync(Guid customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            _context.Customers.Remove(customer);
        }

        public async Task<Customer> GetAsync(Guid customerId)
        {
            return await _context.Customers.FindAsync(customerId);
        }

        public async Task UpdateAsync(Customer customer)
        {
            await Task.Run(() => _context.Customers.Update(customer));
        }
    }
}
