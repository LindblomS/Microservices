namespace Catalog.Application.Services;

using MediatR;
using Microsoft.Extensions.Logging;
using Catalog.Domain.SeedWork;
using System;
using System.Threading.Tasks;

public class DomainEventPublisher
{
    readonly IMediator mediator;
    readonly ILogger<DomainEventPublisher> logger;

    public DomainEventPublisher(IMediator mediator, ILogger<DomainEventPublisher> logger)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task PublishAsync(Entity entity)
    {
        var events = entity.DomainEvents.ToList();
        entity.ClearDomainEvents();

        foreach(var e in events)
        {
            logger.LogInformation("Publishing domain event {DomainEvent} {@DomainEvent}", e.GetType().Name, e);
            await mediator.Publish(e);
        }
    }
}
