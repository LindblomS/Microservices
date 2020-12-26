namespace Services.Customer.API.Application.IntegrationEvents
{
    using EventBus.EventBus.Events;
    using System;
    using System.Threading.Tasks;

    public interface ICustomerIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync(Guid transactionId);
        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}
