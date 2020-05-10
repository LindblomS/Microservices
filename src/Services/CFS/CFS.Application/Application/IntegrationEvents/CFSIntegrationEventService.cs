using EventBus.Abstractions;
using EventBus.Events;
using Microsoft.Extensions.Logging;
using System;

namespace CFS.Application.Application.IntegrationEvents
{
    public class CFSIntegrationEventService : ICFSIntegrationEventService
    {
        private readonly IEventBus _eventBus;
        private ILogger<CFSIntegrationEventService> _logger;

        public CFSIntegrationEventService(IEventBus eventBus, ILogger<CFSIntegrationEventService> logger)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));   
        }

        public void PublishEventsThroughEventBus(IntegrationEvent @event)
        {

            _logger.LogInformation($"----- Publishing integration event: {@event.Id} from {Program.AppName} - ({@event}");

            try
            {
                _eventBus.Publish(@event);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR publishing integration event: {@event.Id} from {Program.AppName}");
            }
        }
    }
}
