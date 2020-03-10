using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBus.Abstractions;
using EventBus.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.API.Application.Commands;
using Ordering.API.Application.IntegrationEvents.Events;
using Serilog.Context;

namespace Ordering.API.Application.IntegrationEvents.EventHandling
{
    public class OrderStockRejectedIntegrationEventHandler : IIntegrationEventHandler<OrderStockRejectedIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrderStockRejectedIntegrationEventHandler> _logger;

        public OrderStockRejectedIntegrationEventHandler(IMediator mediator, ILogger<OrderStockRejectedIntegrationEventHandler> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(OrderStockRejectedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEvent", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation($"----- Handling integration event: {@event.Id} at {Program.AppName} - ({@event})");

                var orderStockRejectedItems = @event.OrderStockItems
                    .FindAll(c => !c.HasStock!)
                    .Select(c => c.ProductId)
                    .ToList();

                var command = new SetStockRejectedOrderStatusCommand(@event.OrderId, orderStockRejectedItems);

                _logger.LogInformation($"----- SendingCommand: {command.GetGenericTypeName()} - {nameof(command.OrderNumber)}: {command.OrderNumber} ({command})");

                await _mediator.Send(command);
            }
        }
    }
}
