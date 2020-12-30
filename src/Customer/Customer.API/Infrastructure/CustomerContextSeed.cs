namespace Services.Customer.API.Infrastructure
{
    using Microsoft.Extensions.Logging;
    using Services.Customer.Infrastructure;
    using Services.Customer.Domain;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;

    public class CustomerContextSeed
    {
        public async Task SeedAsync(CustomerContext context, ILogger<CustomerContextSeed> logger)
        {
            using (context)
            {
                if (!context.Customers.Any())
                {
                    var customers = new List<Customer>
                    {
                        {new Customer(Guid.NewGuid(), "Kalle") },
                        {new Customer(Guid.NewGuid(), "Lisa") },
                        {new Customer(Guid.NewGuid(), "Malin") },
                        {new Customer(Guid.NewGuid(), "Robert") },
                    };

                    await context.Customers.AddRangeAsync(customers);
                    await context.SaveChangesAsync();
                }

            }
        }
    }
}
