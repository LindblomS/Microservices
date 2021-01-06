namespace Services.Order.API.Application.Commands.CommandHandlers
{
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Services.Order.API.Application.Commands.Commands;
    using Services.Order.Domain;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, bool>
    {
        private readonly IOrderRepository _repositroy;
        private readonly ILogger<DeleteOrderCommandHandler> _logger;

        public DeleteOrderCommandHandler(
            IOrderRepository repositroy,
            ILogger<DeleteOrderCommandHandler> logger)
        {
            _repositroy = repositroy ?? throw new ArgumentNullException(nameof(repositroy));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _repositroy.GetAsync(request.OrderId);

            _logger.LogInformation("----- Creating Order - Order: {@Order}", order);

            if (order != null)
            {
                await _repositroy.DeleteAsync(order);
                return await _repositroy.UnitOfWork.SaveEntitiesAsync();
            }
            else
            {
                _logger.LogWarning("----- Order - OrderId: {@OrderId} was not found");
                return false;
            }
        }
    }
}
