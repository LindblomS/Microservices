namespace Catalog.API.Services;

using EventBus.EventBus.Events;

public interface IIntegrationEventService
{
    Task PublishEventsThroughEventBusAsync(Guid transactionId);
    Task AddAndSaveEventAsync(IntegrationEvent e);
}
