namespace Ordering.Application.Services;

using EventBus.EventBus.Events;
using System.Threading.Tasks;

public interface IIntegrationEventService
{
    Task PublishEventsThroughEventBusAsync(Guid transactionId);
    Task AddAndSaveEventAsync(IntegrationEvent e);
}
