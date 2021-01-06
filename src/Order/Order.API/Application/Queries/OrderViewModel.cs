namespace Services.Order.API.Application.Queries
{
    using System;
    using System.Collections.Generic;

    public class OrderViewModel
    {
        public OrderViewModel(Guid orderId, Guid customerId, IEnumerable<OrderItemViewModel> orderItems)
        {
            OrderId = orderId;
            CustomerId = customerId;
            OrderItems = orderItems;
        }

        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public IEnumerable<OrderItemViewModel> OrderItems { get; set; }
    }
}
