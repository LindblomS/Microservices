namespace Ordering.Application.IntegrationEventHandlers;

using EventBus.EventBus.Events;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;

public abstract class BaseIntegrationHandler
{
    readonly ILogger logger;

    public BaseIntegrationHandler(ILogger logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected async Task Handle(Func<Task> action, IntegrationEvent integrationEvent)
    {
        using (LogContext.PushProperty("IntegrationEvent", $"{integrationEvent.Id}-Ordering"))
        {
            logger.LogInformation("Handling integration event {IntegrationEventId} as Ordering - ({@IntegrationEvent})", integrationEvent.Id, integrationEvent);
            try
            {
                await action();
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "");
            }
        }
    }
}
