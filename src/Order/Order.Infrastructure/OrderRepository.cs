namespace Services.Order.Infrastructure
{
    using Services.Order.Domain;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext _context;

        public OrderRepository(OrderContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public IUnitOfWork UnitOfWork => _context;

        public async Task<Order> CreateAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            return order;
        }

        public async Task DeleteAsync(Order order)
        {
            await Task.Run(() => _context.Orders.Remove(order));
        }

        public async Task<Order> GetAsync(Guid orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order != null)
            {
                await _context.Entry(order).Collection(e => e.OrderItems).LoadAsync();
            }

            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersOnCustomer(Guid customerId)
        {
            return await Task.Run(() => _context.Orders.Where(order => order.CustomerId == customerId));
        }

        public async Task UpdateAsync(Order order)
        {
            await Task.Run(() => _context.Orders.Update(order));
        }
    }
}
