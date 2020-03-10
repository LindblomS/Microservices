using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.API.Application.IntegrationEvents;
using Ordering.API.Application.IntegrationEvents.Events;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Infrastructure.Idempotency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Application.Commands.CommandHandlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediator _mediator;
        private readonly IOrderingIntegrationEventService _orderingIntegrationEventService;
        private readonly ILogger<CreateOrderCommandHandler> _logger;

        public CreateOrderCommandHandler(
            IOrderRepository orderRepository,
            IMediator mediator,
            IOrderingIntegrationEventService orderingIntegrationEventService,
            ILogger<CreateOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _orderingIntegrationEventService = orderingIntegrationEventService ?? throw new ArgumentNullException(nameof(orderingIntegrationEventService));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // Add Integration event to clean the basket
            var orderStartedIntegrationEvent = new OrderStartedIntegrationEvent(request.UserId);
            await _orderingIntegrationEventService.AddAndSaveEventAsync(orderStartedIntegrationEvent);

            var address = new Address(request.Street, request.City, request.State, request.Country, request.ZipCode);
            var order = new Order(
                request.UserId, 
                request.UserName, 
                address, 
                request.CardTypeId, 
                request.CardNumber, 
                request.CardSecurityNumber, 
                request.CardHolderName, 
                request.CardExpiration, 
                null, null);

            foreach (var item in request.OrderItems)
            {
                order.AddOrderItem(item.ProductId, item.ProductName, item.PictureUrl, item.UnitPrice, item.Discount, item.Units);
            }

            _logger.LogInformation($"----- Creating Order - Order: {order}");

            _orderRepository.Add(order);

            return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }

    // Use for Idempotency in Command process
    public class CreateOrderIdentifiendCommandHandler : IdentifiedCommandHandler<CreateOrderCommand, bool>
    {
        public CreateOrderIdentifiendCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<CreateOrderCommand, bool>> logger) : base(mediator, requestManager, logger)
        {
        }

        // Ignore duplicate requests for creating order.
        protected override bool CreateResultForDuplicateRequest() => true;
    }
}
