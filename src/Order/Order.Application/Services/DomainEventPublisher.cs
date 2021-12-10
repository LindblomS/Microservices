namespace Ordering.Application.Services;

using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.SeedWork;
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

    public async Task Publish(IAggregateRoot entity)
    {

    }
}
