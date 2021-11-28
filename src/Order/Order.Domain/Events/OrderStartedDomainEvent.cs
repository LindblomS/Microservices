namespace Services.Order.Domain.Events;

using MediatR;

public class OrderStartedDomainEvent : INotification
{

    public OrderStartedDomainEvent(
        AggregateModels.Order.Order order,
        AggregateModels.Order.User user,
        AggregateModels.Order.Card card)
    {
        Order = order;
        User = user;
        Card = card;
    }

    public AggregateModels.Order.Order Order { get; }
    public AggregateModels.Order.User User { get; }
    public AggregateModels.Order.Card Card { get; }
}
