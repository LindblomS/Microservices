namespace Services.Order.Domain.AggregateModels.Order;

using Services.Order.Domain.SeedWork;
using System;
using System.Threading.Tasks;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order> CreateAsync(Order order);
    Task<Order> GetAsync(Guid orderId);
    Task<Order> UpdateAsync(Order order);
}
