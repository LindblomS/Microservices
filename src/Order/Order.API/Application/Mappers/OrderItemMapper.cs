namespace Services.Order.API.Application.Mappers
{
    using Services.Order.API.Application.Commands.Commands;
    using Services.Order.API.Application.Queries;
    using Services.Order.Domain;
    using System;

    public static class OrderItemMapper
    {
        public static OrderItem MapToOrderItem(OrderItemDto orderItemDto, Guid orderId)
        {
            return new OrderItem(orderId, orderItemDto.Name, orderItemDto.Quantity);
        }

        public static OrderItemViewModel MapToOrderItemViewModel(OrderItem orderItem)
        {
            return new OrderItemViewModel(orderItem.Name, orderItem.Quantity);
        }
    }
}
