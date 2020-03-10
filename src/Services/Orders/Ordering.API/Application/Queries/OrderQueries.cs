using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Application.Queries
{
    public class OrderQueries : IOrderQueries
    {
        private string _connectionString = string.Empty;

        public OrderQueries(string connectionString)
        {
            _connectionString = !string.IsNullOrEmpty(connectionString)
                ? connectionString
                : throw new ArgumentNullException(nameof(connectionString));
        }

        public Task<IEnumerable<CardType>> GetCardTypesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrderAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderSummary>> GetOrdersFromUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
