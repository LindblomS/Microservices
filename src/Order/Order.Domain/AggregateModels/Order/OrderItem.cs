namespace Ordering.Domain.AggregateModels.Order;

using Ordering.Domain.SeedWork;
using System;

public class OrderItem : Entity
{
    ProductName productName;
    UnitPrice unitPrice;
    Units units;

    public string ProductName { get => productName.Name; }
    public decimal UnitPrice { get => unitPrice.Price; }
    public int Units { get => units.Value; }

    public OrderItem(Guid id, ProductName productName, UnitPrice unitPrice, Units units)
    {
        if (id == default)
            throw new ArgumentNullException(nameof(id));

        if (productName is null)
            throw new ArgumentNullException(nameof(productName));

        if (unitPrice is null)
            throw new ArgumentNullException(nameof(unitPrice));

        if (units is null)
            throw new ArgumentNullException(nameof(units));

        this.id = id;
        this.productName = productName;
        this.unitPrice = unitPrice;
        this.units = units;
    }

    public void AddUnits(int units)
    {
        this.units.AddUnits(units);
    }
}
