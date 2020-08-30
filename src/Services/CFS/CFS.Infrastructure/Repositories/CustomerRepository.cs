using CFS.Domain.Aggregates;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CFS.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDbWithTransaction _db;

        public CustomerRepository(IDbWithTransaction db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task Add(Customer customer)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO Customers (firstName, lastName, phoneNumber, email, street, city, state, country, zipCode)");
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

            await _db.ExecuteAsync(sql.ToString(), null);
        }

        public async Task<Customer> GetCustomer(int id)
        {
            var sql = $"SELECT * FROM Customers WHERE customerId = @id";
            return await _db.GetAsync<Customer>(sql, new { id });
        }

        public async Task Update(Customer customer)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE Customers SET");
            sql.AppendLine($"firstName = '{customer.FirstName}',");
            sql.AppendLine($"lastName = '{customer.LastName}',");
            sql.AppendLine($"phoneNumber = '{customer.PhoneNumber},'");
            sql.AppendLine($"email = '{customer.Email}',");
            sql.AppendLine($"street = '{customer.Address.Street}',");
            sql.AppendLine($"city = '{customer.Address.City}',");
            sql.AppendLine($"state = '{customer.Address.State}',");
            sql.AppendLine($"country = '{customer.Address.Country}',");
            sql.AppendLine($"zipCode = '{customer.Address.ZipCode}'");

            sql.AppendLine($"WHERE customerId = {customer.Id}");

            await _db.ExecuteAsync(sql.ToString(), null);
        }

        public async Task Delete(int id)
        {
            string sql = $"DELETE FROM Customers WHERE customerId = @id";
            await _db.ExecuteAsync(sql, new { id });
        }
    }
}
