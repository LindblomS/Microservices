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
        if (item is null)
            throw new BasketDomainException("Could not add basket item. Item is missing");

        items.Add(item);
    }

    public void UpdatePrice(Guid productId, decimal newPrice)
    {
        if (productId == default)
            throw new BasketDomainException("Could not update price. Product id was not valid. Product id must be a valid GUID");

        foreach (var item in items.Where(x => x.ProductId == productId))
            item.UpdatePrice(newPrice);
    }
}
