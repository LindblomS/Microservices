using EventBus.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.API.Application.Commands;
using Ordering.API.Application.IntegrationEvents.Events;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBus.Extensions;

namespace Ordering.API.Application.IntegrationEvents.EventHandling
{
    public class OrderPaymentFailedIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentFailedIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrderPaymentFailedIntegrationEventHandler> _logger;

        public OrderPaymentFailedIntegrationEventHandler(IMediator mediator, ILogger<OrderPaymentFailedIntegrationEventHandler> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(OrderPaymentFailedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEvent", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation($"----- Handling integration event: {@event.Id} at {Program.AppName} - ({@event})");

                var command = new CancelOrderCommand(@event.OrderId);

                _logger.LogInformation($"----- SendingCommand: {command.GetGenericTypeName()} - {nameof(command.OrderNumber)}: {command.OrderNumber} ({command})");

                await _mediator.Send(command);
            }
        }
    }
}
