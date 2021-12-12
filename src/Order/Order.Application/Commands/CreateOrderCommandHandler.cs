namespace Ordering.Application.Commands;

using MediatR;
using Ordering.Application.Services;
using Ordering.Contracts.IntegrationEvents;
using Ordering.Domain.AggregateModels.Order;
using System;
using System.Threading;
using System.Threading.Tasks;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
{
    readonly IOrderRepository orderRepository;
    readonly IIntegrationEventService integrationEventService;
    readonly DomainEventPublisher domainEventPublisher;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IIntegrationEventService integrationEventService, DomainEventPublisher domainEventPublisher)
    {
        this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        this.integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        this.domainEventPublisher = domainEventPublisher ?? throw new ArgumentNullException(nameof(domainEventPublisher));
    }

    public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var user = CreateUser(request.UserId, request.Username);
        var card = CreateCard(request.Card);
        var address = CreateAddress(request.Address);
        var order = new Order(Guid.NewGuid(), user, card, address);

        foreach (var item in request.OrderItems)
            order.AddOrderItem(CreateOrderItem(item));

        await orderRepository.AddAsync(order);
        await domainEventPublisher.PublishAsync(order);

        var orderStartedEvent = new OrderStartedIntegrationEvent(request.UserId);
        await integrationEventService.AddAndSaveEventAsync(orderStartedEvent);

        return true;
    }

    User CreateUser(Guid userId, string username)
    {
        return new(userId, username);
    }

    Card CreateCard(CreateOrderCommand.CardDto card)
    {
        return new(card.TypeId, card.Number, card.SecurityNumber, card.HolderName, card.ExpirationDate);
    }

    Address CreateAddress(CreateOrderCommand.AddressDto address)
    {
        return new(address.Street, address.City, address.Street, address.Country, address.ZipCode);
    }

    OrderItem CreateOrderItem(CreateOrderCommand.OrderItem item)
    {
        return new(item.ProductId, new(item.ProductName), new(item.UnitPrice), new(item.Units));
    }
}

public class CreateOrderIdentifiedCommandHandler : IdentifiedCommandHandler<CreateOrderCommand, bool>
{
    public CreateOrderIdentifiedCommandHandler(IRequestManager requestManager, IMediator mediator)
        : base(requestManager, mediator)
    {
    }

    protected override bool CreateResultForDuplicateRequest()
    {
        return true;
    }
}
