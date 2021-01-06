namespace Services.Order.API.Application.IntegrationEvents.Handlers
{
    using EventBus.EventBus.Abstractions;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Serilog.Context;
    using Services.Order.API.Application.Commands.Commands;
    using Services.Order.API.Application.IntegrationEvents.Events;
    using System;
    using System.Threading.Tasks;
    using Services.Order.Domain;

    public class CustomerDeletedIntegrationEventHandler : IIntegrationEventHandler<CustomerDeletedIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly IOrderRepository _repository;
        private readonly ILogger<CustomerDeletedIntegrationEventHandler> _logger;

        public CustomerDeletedIntegrationEventHandler(
            IMediator mediator,
            IOrderRepository repository,
            ILogger<CustomerDeletedIntegrationEventHandler> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(CustomerDeletedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                var orders = await _repository.GetOrdersOnCustomer(@event.CustomerId);

                foreach (var order in orders)
                {
                    var command = new DeleteOrderCommand(order.Id);

                    _logger.LogInformation(
                        "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                        typeof(DeleteOrderCommand).Name,
                        nameof(command.OrderId),
                        command.OrderId,
                        command);

                    await _mediator.Send(command);
                }
            }
        }
    }
}
