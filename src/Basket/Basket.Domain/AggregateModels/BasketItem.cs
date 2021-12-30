namespace Basket.Domain.AggregateModels;

using Domain.Exceptions;
using System;

public class BasketItem
{
    readonly Guid id;
    readonly Guid productId;
    readonly string productName;
    decimal unitPrice;
    decimal oldUnitPrice;
    readonly int units;

    public BasketItem(Guid id, Guid productId, string productName, decimal unitPrice, int units)
    {
        if (id == default)
            throw new ArgumentNullException(nameof(id));

        if (productId == default)
            throw new ArgumentNullException(nameof(productId));

        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentNullException(nameof(productName));

        if (units < 1)
            throw new BasketDomainException("Units cannot be lower than 1");

        this.id = id;
        this.productId = productId;
        this.productName = productName;
        this.unitPrice = unitPrice;
        oldUnitPrice = unitPrice;
        this.units = units;
    }

    public Guid Id { get => id; }
    public Guid ProductId { get => productId; }
    public string ProductName { get => productName; }
    public decimal UnitPrice { get => unitPrice; }
    public decimal OldUnitPrice { get => oldUnitPrice; }
    public int Units { get => units; }

    public void UpdatePrice(decimal newPrice)
    {
        oldUnitPrice = unitPrice;
        unitPrice = newPrice;
    }
}
