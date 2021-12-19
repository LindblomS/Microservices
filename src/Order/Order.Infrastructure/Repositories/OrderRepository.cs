namespace Ordering.Infrastructure.Repositories;

using Ordering.Domain.AggregateModels.Order;
using Ordering.Infrastructure.EntityFramework;
using System;
using System.Threading.Tasks;

public class OrderRepository : IOrderRepository
{
    readonly OrderingContext context;

    public OrderRepository(OrderingContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Order> AddAsync(Order order)
    {
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
