namespace Services.Order.API.Application.Commands.CommandHandlers
{
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Services.Order.API.Application.Commands.Commands;
    using Services.Order.API.Application.Mappers;
    using Services.Order.Domain;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
    {
        private readonly IOrderRepository _repositroy;
        private readonly ILogger<CreateOrderCommandHandler> _logger;

        public CreateOrderCommandHandler(
            IOrderRepository repositroy,
            ILogger<CreateOrderCommandHandler> logger)
        {
            _repositroy = repositroy ?? throw new ArgumentNullException(nameof(repositroy));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderId = Guid.NewGuid();
            var order = new Order(orderId, request.CustomerId);
            foreach (var orderItem in request.OrderItems)
            {
                order.AddOrderItem(OrderItemMapper.MapToOrderItem(orderItem, orderId));
            }

            _logger.LogInformation("----- Creating Order - Order: {@Order}", order);

            await _repositroy.CreateAsync(order);
            await _repositroy.UnitOfWork.SaveEntitiesAsync();

            return true;
        }
    }
}
