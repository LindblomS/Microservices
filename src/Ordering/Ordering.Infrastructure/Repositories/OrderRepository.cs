namespace Ordering.Infrastructure.Repositories;

using Ordering.Domain.AggregateModels.Order;
using Ordering.Infrastructure.EntityFramework;
using Ordering.Infrastructure.Mappers;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
        var entity = await context.Orders.FindAsync(orderId.ToString());
        var orderItems = context.OrderItems.Where(x => x.OrderId == orderId.ToString());

        entity.OrderItems = orderItems.ToList();

        entity.Buyer = string.IsNullOrEmpty(entity.BuyerId) 
            ? null 
            : await context.Buyers.FindAsync(entity.BuyerId.ToString());

        entity.Card = string.IsNullOrEmpty(entity.CardId)
            ? null
            : await context.Cards.FindAsync(entity.CardId.ToString());

        return OrderMapper.Map(entity);
    }

    public async Task UpdateAsync(Order order)
    {
        var updated = OrderMapper.Map(order);
        var original = await context.Orders.FindAsync(order.Id.ToString());
        context.Entry(original).CurrentValues.SetValues(updated);
        await context.SaveChangesAsync();
    }
}
