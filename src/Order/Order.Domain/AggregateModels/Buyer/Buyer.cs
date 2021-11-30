namespace Ordering.Domain.AggregateModels.Buyer;

using Ordering.Domain.Events;
using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

public class Buyer : Entity, IAggregateRoot
{
    string name;
    List<Card> cards;

    Buyer(Guid id)
    {
        if (id == default)
            throw new ArgumentNullException(nameof(id));

        this.id = id;
        cards = new();
    }

    public Buyer(Guid id, string name) : this(id)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));

        this.name = name;
    }

    public string Name { get => name; }
    public IReadOnlyCollection<Card> Cards { get => cards.AsReadOnly(); }

    public void VerifyOrAddCard(Card card, Guid orderId)
    {
        var existingCard = cards.SingleOrDefault(x => x == card);

        if (existingCard is null)
            cards.Add(card);

        AddDomainEvent(new BuyerAndCardVerifiedDomainEvent(this, card, orderId));
    }
}
