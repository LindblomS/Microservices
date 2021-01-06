namespace Services.Order.API.Application.IntegrationEvents
{
    using EventBus.EventBus.Events;
    using System;
    using System.Threading.Tasks;

    public interface IOrderIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync(Guid transactionId);
        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}
