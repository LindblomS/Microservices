namespace Ordering.Infrastructure.Repositories;

using Ordering.Domain.AggregateModels.Order;
using System;
using System.Threading.Tasks;

public class OrderRepository : IOrderRepository
{
    public Task<Order> AddAsync(Order order)
    {
        throw new NotImplementedException();
    }

    public Task<Order> GetAsync(Guid orderId)
    {
        throw new NotImplementedException();
    }

    public Task<Order> UpdateAsync(Order order)
    {
        throw new NotImplementedException();
    }
}
