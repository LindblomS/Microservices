namespace Ordering.Application.RequestHandlers.CommandHandlers;

using MediatR;
using Ordering.Application.Services;
using Ordering.Application.Commands;
using Ordering.Contracts.IntegrationEvents;
using Ordering.Domain.AggregateModels.Order;
using System;
using System.Threading;
using System.Threading.Tasks;

public class CreateOrderHandler : IRequestHandler<CreateOrder, bool>
{
    readonly IOrderRepository orderRepository;
    readonly IIntegrationEventService integrationEventService;
    readonly DomainEventPublisher domainEventPublisher;

    public CreateOrderHandler(IOrderRepository orderRepository, IIntegrationEventService integrationEventService, DomainEventPublisher domainEventPublisher)
    {
        this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        this.integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        this.domainEventPublisher = domainEventPublisher ?? throw new ArgumentNullException(nameof(domainEventPublisher));
    }

    public async Task<bool> Handle(CreateOrder request, CancellationToken cancellationToken)
    {
        var user = CreateUser(request.UserId, request.Username);
        var card = CreateCard(request.Card);
        var address = CreateAddress(request.Address);
        var order = new Order(Guid.NewGuid(), user, card, address);

        foreach (var item in request.OrderItems)
            order.AddOrderItem(CreateOrderItem(item));

        await orderRepository.AddAsync(order);

        var orderStartedEvent = new OrderStartedIntegrationEvent(request.UserId);
        await integrationEventService.AddAndSaveEventAsync(orderStartedEvent);

        await domainEventPublisher.PublishAsync(order);

        return true;
    }

    User CreateUser(Guid userId, string username)
    {
        return new(userId, username);
    }

    Card CreateCard(CreateOrder.CardDto card)
    {
        return new(card.TypeId, card.Number, card.SecurityNumber, card.HolderName, card.ExpirationDate);
    }

    Address CreateAddress(CreateOrder.AddressDto address)
    {
        return new(address.Street, address.City, address.State, address.Country, address.ZipCode);
    }

    OrderItem CreateOrderItem(CreateOrder.OrderItem item)
    {
        return new(item.ProductId, new(item.ProductName), new(item.UnitPrice), new(item.Units));
    }
}

public class CreateOrderIdentifiedCommandHandler : IdentifiedCommandHandler<CreateOrder, bool>
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
