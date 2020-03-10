using EventBus.Abstractions;
using EventBus.Events;
using IntegrationEventLogEF;
using IntegrationEventLogEF.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Application.IntegrationEvents
{
    public class OrderingIntegrationEventService : IOrderingIntegrationEventService
    {
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IEventBus _eventBus;
        private readonly OrderingContext _orderingContext;
        private readonly IIntegrationEventLogService _eventLogService;
        private ILogger<OrderingIntegrationEventService> _logger;

        public OrderingIntegrationEventService(
            IEventBus eventBus,
            OrderingContext orderingContext,
            IntegrationEventLogContext eventLogContext,
            Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory,
            ILogger<OrderingIntegrationEventService> logger)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _orderingContext = orderingContext ?? throw new ArgumentNullException(nameof(orderingContext));
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? 
                throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            // Kanske ska vara eventlogcontext här istället för orderingContext
            _eventLogService = _integrationEventLogServiceFactory(orderingContext.Database.GetDbConnection());

        }

        public async Task AddAndSaveEventAsync(IntegrationEvent @event)
        {
            _logger.LogInformation($"----- Enqueing integration event {@event.Id} to repository ({@event})");
            await _eventLogService.SaveEventAsync(@event, _orderingContext.GetCurrentTransaction());
        }

        public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
        {
            var pendingLogEvents = await _eventLogService.RetriveEventLogsPendingToPublishAsync(transactionId);

            foreach (var logEvent in pendingLogEvents)
            {
                _logger.LogInformation($"----- Publishing integration event: {logEvent.EventId} from {Program.AppName} - ({logEvent.IntegrationEvent})");
                try
                {
                    await _eventLogService.MarkEventAsInProgressAsync(logEvent.EventId);
                    _eventBus.Publish(logEvent.IntegrationEvent);
                    await _eventLogService.MarkEventAsPublishedAsync(logEvent.EventId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"ERROR publishing integration event: {logEvent.EventId} from {Program.AppName}");
                    await _eventLogService.MarkEventAsFailedAsync(logEvent.EventId);
                }
            }
        }
    }
}
