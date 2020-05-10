using EventBus.Events;
using System.Threading.Tasks;

namespace CFS.Application.Application.IntegrationEvents
{
    public interface ICFSIntegrationEventService
    {
        void PublishEventsThroughEventBus(IntegrationEvent @event);
    }
}
