namespace Basket.Domain.AggregateModels;

using Domain.Exceptions;
using System;

public class BasketItem
{
    readonly Guid productId;
    readonly string productName;
    decimal unitPrice;
    decimal oldUnitPrice;
    readonly int units;

    public BasketItem(Guid productId, string productName, decimal unitPrice, decimal oldUnitPrice, int units)
    {
        if (productId == default)
            throw new CreateBasketItemException("Product id was not valid. Product id must be a vaild GUID");

        if (string.IsNullOrWhiteSpace(productName))
            throw new CreateBasketItemException("Product name was missing");

        if (units < 1)
            throw new CreateBasketItemException($"Units cannot be lower than 1. Units was {units}");

        this.productId = productId;
        this.productName = productName;
        this.unitPrice = unitPrice;
        this.oldUnitPrice = oldUnitPrice;
        this.units = units;
    }

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
