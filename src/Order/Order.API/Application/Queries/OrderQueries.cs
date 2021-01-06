namespace Services.Order.API.Application.Queries
{
    using Services.Order.API.Application.Mappers;
    using Services.Order.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class OrderQueries : IOrderQueries
    {
        private readonly OrderContext _context;

        public OrderQueries(OrderContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<OrderViewModel>> GetOrdersAsync()
        {
            return await Task.Run(() =>
            {
                var orders = new List<OrderViewModel>();

                foreach (var order in _context.Orders)
                {
                    orders.Add(OrderMapper.MapToOrderViewModel(order));
                }

                return orders;
            });

        }
    }
}
