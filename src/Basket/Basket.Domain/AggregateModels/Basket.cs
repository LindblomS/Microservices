namespace Basket.Domain.AggregateModels;

using Domain.SeedWork;
using System;
using System.Collections.Generic;

public class Basket : IAggregateRoot
{
    readonly Guid id;
    readonly Guid buyerId;
    readonly List<BasketItem> items;

    public Basket(Guid id, Guid buyerId)
    {
        if (id == default)
            throw new ArgumentNullException(nameof(id));

        if (buyerId == default)
            throw new ArgumentNullException(nameof(buyerId));

        this.id = id;
        this.buyerId = buyerId;
        items = new List<BasketItem>();
    }

    public Guid Id { get => id; }
    public Guid BuyerId { get => buyerId; }
    public IReadOnlyCollection<BasketItem> Items { get => items.AsReadOnly(); }

    public void AddBasketItem(BasketItem item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        items.Add(item);
    }

    public void UpdatePrice(Guid productId, decimal newPrice)
    {
        if (productId == default)
            throw new ArgumentNullException(nameof(productId));

        foreach (var item in items.Where(x => x.ProductId == productId))
            item.UpdatePrice(newPrice);
    }
}
