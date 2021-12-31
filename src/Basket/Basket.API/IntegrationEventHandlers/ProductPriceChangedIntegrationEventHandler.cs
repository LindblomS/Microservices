namespace Basket.API.IntegrationEventHandlers;

using Basket.Domain.AggregateModels;
using Catalog.Contracts.IntegrationEvents;
using EventBus.EventBus.Abstractions;
using Serilog.Context;
using System.Threading.Tasks;

public class ProductPriceChangedIntegrationEventHandler : IIntegrationEventHandler<ProductPriceChangedIntegrationEvent>
{
    readonly IBasketRepository repository;
    readonly ILogger<ProductPriceChangedIntegrationEventHandler> logger;

    public ProductPriceChangedIntegrationEventHandler(IBasketRepository repository, ILogger<ProductPriceChangedIntegrationEventHandler> logger)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(ProductPriceChangedIntegrationEvent @event)
    {
        using (LogContext.PushProperty("IntegrationEvent", $"{@event.Id}-Basket"))
        {
            logger.LogInformation("Handling integration event {IntegrationEventId} as Basket - ({@IntegrationEvent})", @event.Id, @event);
            var userIds = await repository.GetUsersAsync();

            foreach (var id in userIds)
            {
                var basket = await repository.GetBasketAsync(id);

                if (basket is null)
                    return;

                basket.UpdatePrice(@event.ProductId, @event.NewPrice);
            }
        }
    }
}
