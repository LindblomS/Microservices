namespace Basket.API.IntegrationEventHandlers;

using Basket.Domain.AggregateModels;
using EventBus.EventBus.Abstractions;
using Ordering.Contracts.IntegrationEvents;
using Serilog.Context;
using System.Threading.Tasks;

public class OrderStartedIntegrationEventHandler : IIntegrationEventHandler<OrderStartedIntegrationEvent>
{
    readonly IBasketRepository repository;
    readonly ILogger<OrderStartedIntegrationEventHandler> logger;

    public OrderStartedIntegrationEventHandler(IBasketRepository repository, ILogger<OrderStartedIntegrationEventHandler> logger)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(OrderStartedIntegrationEvent @event)
    {
        using (LogContext.PushProperty("IntegrationEvent", $"{@event.Id}-Basket"))
        {
            logger.LogInformation("Handling integration event {IntegrationEventId} as Basket - ({@IntegrationEvent})", @event.Id, @event);
            await repository.DeleteBasketAsync(@event.UserId);
        }
    }
}
