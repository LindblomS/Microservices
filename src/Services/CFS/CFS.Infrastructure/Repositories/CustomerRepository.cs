using CFS.Domain.Aggregates.CustomerAggregate;
using CFS.Domain.SeedWork;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CFS.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DataContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public CustomerRepository(DataContext dataContext)
        {
            _context = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public async Task Add(Customer customer)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO CFS.Customers (firstName, lastName, phoneNumber, email, street, city, state, country, zipCode)");
            sql.AppendLine(string.Format("VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}')", 
                customer.FirstName,
                customer.LastName, 
                customer.PhoneNumber, 
                customer.Email, 
                customer.Address.Street, 
                customer.Address.City, 
                customer.Address.State, 
                customer.Address.Country, 
                customer.Address.ZipCode));

            await _context.ExecuteAsync(sql.ToString());
        }

        public async Task<Customer> GetCustomer(int customerId)
        {
            var sql = $"SELECT * FROM CFS.Customers WHERE customerId = {customerId}";
            return await _context.QueryAsync<Customer>(sql);
        }

        public async Task Update(Customer customer)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE CFS.Customers SET");
            sql.AppendLine($"firstName = '{customer.FirstName}'");
            sql.AppendLine($"lastName = '{customer.LastName}'");
            sql.AppendLine($"phoneNumber = '{customer.PhoneNumber}'");
            sql.AppendLine($"email = '{customer.Email}'");
            sql.AppendLine($"street = '{customer.Address.Street}'");
            sql.AppendLine($"city = '{customer.Address.City}'");
            sql.AppendLine($"state = '{customer.Address.State}'");
            sql.AppendLine($"country = '{customer.Address.Country}'");
            sql.AppendLine($"zipCode = '{customer.Address.ZipCode}'");

            sql.AppendLine($"WHERE customerId = {customer.Id}");

            await _context.ExecuteAsync(sql.ToString());
        }
    }
}
