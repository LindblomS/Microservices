namespace Order.Application.IntegrationEvents;

using EventBus.EventBus.Events;
using System;
using System.Threading.Tasks;

internal interface IIntegrationEventService
{
    Task PublishEventsThroughEventBusAsync(Guid transactionId);
    Task AddAndSaveEventAsync(IntegrationEvent e);
}
