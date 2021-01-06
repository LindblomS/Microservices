namespace Services.Customer.API.Application.Handlers.DomainEventHandlers
{
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Services.Customer.API.Application.IntegrationEvents;
    using Services.Customer.API.Application.IntegrationEvents.Events;
    using Services.Customer.Domain;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class CustomerDeletedDomainEventHandler : INotificationHandler<CustomerDeletedDomainEvent>
    {
        private readonly ICustomerIntegrationEventService _integrationEventService;
        private readonly ICustomerRepository _repository;
        private readonly ILogger<CustomerDeletedDomainEventHandler> _logger;

        public CustomerDeletedDomainEventHandler(
            ICustomerIntegrationEventService integrationEventService, 
            ICustomerRepository repository,
            ILogger<CustomerDeletedDomainEventHandler> logger)
        {
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(CustomerDeletedDomainEvent notification, CancellationToken cancellationToken)
        {
            var customer = await _repository.GetAsync(notification.CustomerId);
            var @event = new CustomerDeletedIntegrationEvent(customer.Id);
            await _integrationEventService.AddAndSaveEventAsync(@event);
        }
    }
}
