namespace Services.Customer.API.Application.Queries
{
    using Services.Customer.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CustomerQueries : ICustomerQueries
    {
        private readonly CustomerContext _context;

        public CustomerQueries(CustomerContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IList<CustomerViewModel>> GetAsync()
        {
            return await Task.Run(() =>
            {
                var customers = new List<CustomerViewModel>();
                foreach (var customer in _context.Customers)
                {
                    customers.Add(new CustomerViewModel(customer.Id, customer.Name));
                }
                return customers;
            });
        }

        public async Task<CustomerViewModel> GetAsync(Guid customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            return new CustomerViewModel(customer.Id, customer.Name);
        }
    }
}
