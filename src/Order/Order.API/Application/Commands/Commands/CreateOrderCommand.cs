namespace Services.Order.API.Application.Commands.Commands
{
    using MediatR;
    using System;
    using System.Collections.Generic;

    public class CreateOrderCommand : IRequest<bool>
    {
        public CreateOrderCommand(Guid customerId, IEnumerable<OrderItemDto> orderItems)
        {
            CustomerId = customerId;
            OrderItems = orderItems;
        }

        public Guid CustomerId { get; private set; }
        public IEnumerable<OrderItemDto> OrderItems { get; private set; }
    }

    public class OrderItemDto
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
