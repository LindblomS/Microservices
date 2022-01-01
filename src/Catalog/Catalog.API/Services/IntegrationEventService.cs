namespace Catalog.API.Services;

using EventBus.EventBus.Events;
using System;
using System.Threading.Tasks;

public class IntegrationEventService : IIntegrationEventService
{
    public Task AddAndSaveEventAsync(IntegrationEvent e)
    {
        throw new NotImplementedException();
    }

    public Task PublishEventsThroughEventBusAsync(Guid transactionId)
    {
        throw new NotImplementedException();
    }
}
