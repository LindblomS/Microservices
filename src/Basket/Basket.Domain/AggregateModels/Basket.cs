namespace Basket.Domain.AggregateModels;

using Domain.SeedWork;
using Domain.Exceptions;
using System;
using System.Collections.Generic;

public class Basket : IAggregateRoot
{
    readonly Guid buyerId;
    readonly List<BasketItem> items;

    public Basket(Guid buyerId)
    {
        if (buyerId == default)
            throw new CreateBasketException("Buyer id was not valid. Buyer id must be a valid GUID");

        this.buyerId = buyerId;
        items = new List<BasketItem>();
    }

    public Guid BuyerId { get => buyerId; }
    public IReadOnlyCollection<BasketItem> Items { get => items.AsReadOnly(); }

    public void AddBasketItem(BasketItem item)
    {
        var existingItem = items.SingleOrDefault(x => x.ProductId == item.ProductId);

        if (existingItem is null)
            items.Add(item);

        item.AddUnit();
    }
}
