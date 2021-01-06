namespace Services.Order.Domain
{
    using Services.Order.Domain.SeedWork;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order> CreateAsync(Order order);
        Task<Order> GetAsync(Guid orderId);
        Task<IEnumerable<Order>> GetOrdersOnCustomer(Guid customerId);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Order order);
    }
}
