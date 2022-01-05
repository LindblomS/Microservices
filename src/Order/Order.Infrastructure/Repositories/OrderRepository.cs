namespace Ordering.Infrastructure.Repositories;

using Ordering.Domain.AggregateModels.Order;
using Ordering.Infrastructure.EntityFramework;
using Ordering.Infrastructure.Mappers;
using System;
using System.Threading.Tasks;

public class OrderRepository : IOrderRepository
{
    readonly OrderingContext context;

    public OrderRepository(OrderingContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddAsync(Order order)
    {
        await context.Orders.AddAsync(OrderMapper.Map(order));
        await context.SaveChangesAsync();
    }

    public async Task<Order> GetAsync(Guid orderId)
    {
        var entity = await context.Orders.FindAsync(orderId);
        return OrderMapper.Map(entity);
    }

    public async Task UpdateAsync(Order order)
    {
        var updated = OrderMapper.Map(order);
        var original = await context.Orders.FindAsync(order.Id);
        context.Entry(original).CurrentValues.SetValues(updated);
        await context.SaveChangesAsync();
    }
}
