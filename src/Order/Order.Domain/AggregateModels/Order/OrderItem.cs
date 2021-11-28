namespace Services.Order.Domain.AggregateModels.Order;

using Services.Order.Domain.Exceptions;
using Services.Order.Domain.SeedWork;
using System;

public class OrderItem : Entity
{
    string productName;
    decimal unitPrice;
    int units;

    public string ProductName { get => productName; }
    public decimal UnitPrice { get => unitPrice; }
    public int Units { get => units; }

    public OrderItem(Guid id, string productName, decimal unitPrice, int units)
    {
        if (id == default)
            throw new OrderingDomainException("invalid id");

        if (string.IsNullOrEmpty(productName))
            throw new OrderingDomainException("product name cannot be empty");

        if (unitPrice < 1)
            throw new OrderingDomainException($"Unit price cannot be lower than 1. Unit price was {unitPrice}");

        if (units < 0)
            throw new OrderingDomainException($"Units cannot be less than 0. Units was {units}");

        this.id = id;
        this.productName = productName;
        this.unitPrice = unitPrice;
        this.units = units;
    }

    public void AddUnits(int units)
    {
        if (units > 0)
            throw new OrderingDomainException("Invalid units");

        this.units += units;
    }

   
}
