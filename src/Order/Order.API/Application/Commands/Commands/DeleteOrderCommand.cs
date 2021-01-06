namespace Services.Order.API.Application.Commands.Commands
{
    using MediatR;
    using System;

    public class DeleteOrderCommand : IRequest<bool>
    {
        public DeleteOrderCommand(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; private set; }
    }
}
