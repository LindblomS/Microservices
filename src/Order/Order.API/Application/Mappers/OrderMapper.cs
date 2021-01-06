namespace Services.Order.API.Application.Mappers
{
    using Services.Order.API.Application.Queries;
    using Services.Order.Domain;
    using System.Collections.Generic;

    public static class OrderMapper
    {
        public static OrderViewModel MapToOrderViewModel(Order order)
        {
            var orderItems = new List<OrderItemViewModel>();
            foreach (var orderItem in order.OrderItems)
            {
                orderItems.Add(OrderItemMapper.MapToOrderItemViewModel(orderItem));
            }
            return new OrderViewModel(order.Id, order.CustomerId, orderItems);
        }
    }
}
