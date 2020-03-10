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
    public class GracePeriodConfirmedIntegrationEventHandler : IIntegrationEventHandler<GracePeriodConfirmedIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<GracePeriodConfirmedIntegrationEventHandler> _logger;

        public GracePeriodConfirmedIntegrationEventHandler(IMediator mediator, ILogger<GracePeriodConfirmedIntegrationEventHandler> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(GracePeriodConfirmedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation($"----- Handling integration event: {@event.Id} at {Program.AppName} - ({@event})");

                var command = new SetAwaitingValidationOrderStatusCommand(@event.OrderId);

                _logger.LogInformation($"----- SendingCommand: {command.GetGenericTypeName()} - {nameof(command.OrderNumber)}: {command.OrderNumber} ({command})");

                await _mediator.Send(command);
            }
        }
    }
}
