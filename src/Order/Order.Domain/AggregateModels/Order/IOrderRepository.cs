namespace Ordering.Domain.AggregateModels.Order;

using Ordering.Domain.SeedWork;
using System;
using System.Threading.Tasks;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order> AddAsync(Order order);
    Task<Order> GetAsync(Guid orderId);
}
