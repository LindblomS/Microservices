namespace Payment.API.IntegrationEventHandlers;

using EventBus.EventBus.Abstractions;
using Ordering.Contracts.IntegrationEvents;
using System.Threading.Tasks;

public class OrderStatusChangedToStockConfirmedIntegrationEventHandler : IIntegrationEventHandler<OrderStatusChangedToStockConfirmedIntegrationEvent>
{
    readonly IEventBus eventBus;
    readonly ILogger<OrderStatusChangedToStockConfirmedIntegrationEventHandler> logger;

    public OrderStatusChangedToStockConfirmedIntegrationEventHandler(
        IEventBus eventBus,
        ILogger<OrderStatusChangedToStockConfirmedIntegrationEventHandler> logger)
    {
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public Task Handle(OrderStatusChangedToStockConfirmedIntegrationEvent @event)
    {
        throw new NotImplementedException();
    }
}
