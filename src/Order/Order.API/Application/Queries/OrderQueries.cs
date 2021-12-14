namespace Services.Order.API.Application.Queries
{
    using Services.Order.API.Application.Mappers;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using Ordering.Infrastructure;

    public class OrderQueries : IOrderQueries
    {
        private readonly OrderContext _context;

        public OrderQueries(OrderContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<OrderViewModel>> GetOrdersAsync(Guid customerId)
        {
            return await Task.Run(() =>
            {
                var orders = new List<OrderViewModel>();

                foreach (var order in _context.Orders.Where(x => x.CustomerId == customerId))
                    orders.Add(OrderMapper.MapToOrderViewModel(order));

                return orders;
            });

        }
    }
}
