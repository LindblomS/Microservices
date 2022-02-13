namespace Catalog.Application.DomainEventHandlers.CatalogItemPriceChanged;

using Catalog.Contracts.IntegrationEvents;
using Catalog.Domain.Events;
using EventBus.EventBus.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

public class PublishIntegrationEventDomainEventHandler : INotificationHandler<CatalogItemPriceChangedDomainEvent>
{
    readonly IEventBus eventBus;
    readonly ILogger<PublishIntegrationEventDomainEventHandler> logger;

    public PublishIntegrationEventDomainEventHandler(
        IEventBus eventBus,
        ILogger<PublishIntegrationEventDomainEventHandler> logger)
    {
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task Handle(CatalogItemPriceChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new ProductPriceChangedIntegrationEvent(notification.Id, notification.NewPrice);

        try
        {
            logger.LogInformation("Publishing integration event: {IntegrationId} from Catalog", integrationEvent.Id);
            eventBus.Publish(integrationEvent);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error publishing integration event: {IntegrationEventId} from Catalog", integrationEvent.Id);
        }

        return Task.CompletedTask;
    }
}
