namespace Ordering.Infrastructure.Services;

using EventBus.EventBus.Abstractions;
using EventBus.EventBus.Events;
using EventBus.IntegrationEventLogEF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Application.Services;
using Ordering.Infrastructure.EntityFramework;
using System;
using System.Data;
using System.Threading.Tasks;

public class IntegrationEventService : IIntegrationEventService
{
    readonly IEventBus eventBus;
    readonly OrderingContext context;
    readonly IIntegrationEventLogService eventLogService;
    readonly ILogger<IntegrationEventService> logger;

    public IntegrationEventService(
        IEventBus eventBus,
        Func<IDbConnection, IIntegrationEventLogService> eventLogServiceFactory,
        OrderingContext context,
        ILogger<IntegrationEventService> logger)
    {
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        this.context = context ?? throw new ArgumentNullException(nameof(context));

        eventLogService =
            eventLogServiceFactory?.Invoke(context.Database.GetDbConnection()) ??
            throw new ArgumentNullException(nameof(context));

        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task AddAndSaveEventAsync(IntegrationEvent e)
    {
        logger.LogInformation("Enqueuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})", e.Id, e);
        await eventLogService.SaveEventAsync(e, context.GetCurrentTransaction());
    }

    public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
    {
        var pendingLogEvents = await eventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId);

        foreach (var logEvent in pendingLogEvents)
        {
            logger.LogInformation("Publishing integration event: {IntegrationEventId} from Ordering", logEvent.EventId);

            try
            {
                await eventLogService.MarkEventAsInProgressAsync(logEvent.EventId);
                eventBus.Publish(logEvent.IntegrationEvent);
                await eventLogService.MarkEventAsPublishedAsync(logEvent.EventId);

            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Error publishing integration event: {IntegrationEventId} from Ordering", logEvent.EventId);
            }
        }
    }
}
