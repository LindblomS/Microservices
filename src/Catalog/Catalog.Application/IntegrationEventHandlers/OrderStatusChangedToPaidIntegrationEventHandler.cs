﻿namespace Catalog.Application.IntegrationEventHandlers;

using Catalog.Domain.Aggregates;
using EventBus.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using Ordering.Contracts.IntegrationEvents;
using Serilog.Context;
using System.Threading.Tasks;

public class OrderStatusChangedToPaidIntegrationEventHandler : IIntegrationEventHandler<OrderStatusChangedToPaidIntegrationEvent>
{
    readonly ICatalogRepository repository;
    readonly ILogger<OrderStatusChangedToPaidIntegrationEventHandler> logger;

    public OrderStatusChangedToPaidIntegrationEventHandler(
        ICatalogRepository repository,
        ILogger<OrderStatusChangedToPaidIntegrationEventHandler> logger)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public async Task Handle(OrderStatusChangedToPaidIntegrationEvent @event)
    {
        using (LogContext.PushProperty("IntegrationEvent", $"{@event.Id}-Basket"))
        {
            logger.LogInformation("Handling integration event {IntegrationEventId} as Catalog - ({@IntegrationEvent})", @event.Id, @event);

            try
            {
                foreach (var item in @event.OrderItems)
                {
                    var catalogItem = await repository.GetAsync(item.ProductId);

                    if (catalogItem is null)
                    {
                        logger.LogWarning("Catalog item {CatalogId} did not exists", item.ProductId);
                        continue;
                    }

                    catalogItem.RemoveStock(item.Units);
                    await repository.UpdateAsync(catalogItem);
                }
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Error handling integration event {IntegrationEventId}", @event.Id);
            }
        }
    }
}
