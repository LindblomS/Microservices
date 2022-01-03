namespace Payment.API.IntegrationEventHandlers;

using EventBus.EventBus.Abstractions;
using EventBus.EventBus.Events;
using Ordering.Contracts.IntegrationEvents;
using Payment.Contracts.IntegrationEvents;
using Serilog.Context;
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
        using (LogContext.PushProperty("IntegrationEvent", $"{@event.Id}-Payment"))
        {
            logger.LogInformation("Handling integration event {IntegrationEventId} as Payment - ({@IntegrationEvent})", @event.Id, @event);
            try
            {
                var random = new Random().Next(1, 3);


                IntegrationEvent integrationEvent = random switch
                {
                    1 => new OrderPaymentFailedIntegrationEvent(@event.OrderId),
                    _ => new OrderPaymentSucceededIntegrationEvent(@event.OrderId),
                };

                logger.LogInformation("Publishing integration event: {IntegrationEventId} from Payment", integrationEvent.Id);
                eventBus.Publish(integrationEvent);
            }
            catch
            {
            }

            return Task.CompletedTask;
        }
    }
}
