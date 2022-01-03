namespace Catalog.API.IntegrationHandlers;

using Catalog.Contracts.IntegrationEvents;
using Catalog.Domain.Aggregates;
using EventBus.EventBus.Abstractions;
using EventBus.EventBus.Events;
using Ordering.Contracts.IntegrationEvents;
using Serilog.Context;
using System.Threading.Tasks;

public class OrderStatusChangedToAwaitingValidationIntegrationEventHandler : IIntegrationEventHandler<OrderStatusChangedToAwaitingValidationIntegrationEvent>
{
    readonly ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> logger;
    readonly ICatalogRepository repository;
    readonly IEventBus eventBus;

    public OrderStatusChangedToAwaitingValidationIntegrationEventHandler(
        ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> logger,
        ICatalogRepository repository,
        IEventBus eventBus)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }
    public async Task Handle(OrderStatusChangedToAwaitingValidationIntegrationEvent @event)
    {
        using (LogContext.PushProperty("IntegrationEvent", $"{@event.Id}-Basket"))
        {
            logger.LogInformation("Handling integration event {IntegrationEventId} as Catalog - ({@IntegrationEvent})", @event.Id, @event);

            try
            {
                var items = new Dictionary<Guid, bool>();

                foreach (var item in @event.OrderItems)
                {
                    var catalogItem = await repository.GetAsync(item.ProductId);
                    
                    if (catalogItem is null)
                    {
                        logger.LogWarning("Catalog item {CatalogId} did not exists", item.ProductId);
                        items.Add(item.ProductId, false);
                        continue;
                    }

                    if (catalogItem.AvailableStock >= item.Units)
                        items.Add(item.ProductId, true);
                }

                IntegrationEvent integrationEvent = items.Any(x => !x.Value) switch
                {
                    true => new OrderStockRejectedIntegrationEvent(
                        @event.OrderId, 
                        new List<OrderStockRejectedIntegrationEvent.OrderItem>(
                            items.Select(x => new OrderStockRejectedIntegrationEvent.OrderItem(x.Key, x.Value)))),
                    _ => new OrderStockConfirmedIntegrationEvent(@event.OrderId)
                };

                logger.LogInformation("Publishing integration event: {IntegrationEventId} from Catalog", integrationEvent.Id);
                eventBus.Publish(integrationEvent);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Error handling integration event {IntegrationEventId}", @event.Id);
            }
        }
    }
}
