namespace Catalog.Domain.Aggregates;

using Catalog.Domain.Events;
using Catalog.Domain.Exceptions;
using Catalog.Domain.SeedWork;
using System;

public class CatalogItem : Entity, IAggregateRoot
{
    readonly Guid id;
    readonly string name;
    readonly string description;
    decimal price;
    readonly CatalogType type;
    readonly CatalogBrand brand;
    int availableStock;

    public CatalogItem(
        Guid id,
        string name,
        string description,
        decimal price,
        CatalogType type,
        CatalogBrand brand,
        int availableStock)
    {
        if (id == default)
            throw new CreateCatalogItemException("Id was not valid");

        if (string.IsNullOrWhiteSpace(name))
            throw new CreateCatalogItemException("Name was empty");

        if (string.IsNullOrWhiteSpace(description))
            throw new CreateCatalogItemException("Description was empty");

        if (price < 1)
            throw new CreateCatalogItemException($"Price must be greater than 0. Price was {price}");

        if (type is null)
            throw new CreateCatalogItemException("Catalog type was missing");

        if (brand is null)
            throw new CreateCatalogItemException("Catalog brand was missing");

        if (availableStock < 0)
            throw new CreateCatalogItemException($"Available stock cannot be less than 0. Available stock was {availableStock}");

        this.id = id;
        this.name = name;
        this.description = description;
        this.price = price;
        this.type = type;
        this.brand = brand;
        this.availableStock = availableStock;
    }

    public Guid Id { get => id; }
    public string Name { get => name; }
    public string Description { get => description; }
    public decimal Price { get => price; }
    public CatalogType Type { get => type; }
    public CatalogBrand Brand { get => brand; }
    public int AvailableStock { get => availableStock; }

    public void RemoveStock(int units)
    {
        if (availableStock == 0)
            throw new CatalogDomainException($"Cannot remove stock. Stock is empty for {name}");

        if (units < 1)
            throw new CatalogDomainException($"Cannot remove stock. Units must be greater than 0. Units was {units}");

        availableStock -= units;
    }

    public void AddStock(int units)
    {
        if (units < 1)
            throw new CatalogDomainException($"Cannot add stock. Units must be greater than 0. Units was {units}");

        availableStock += units;
    }

    public void ChangePrice(decimal newPrice)
    {
        if (price == newPrice)
            return;

        price = newPrice;
        AddDomainEvent(new CatalogItemPriceChangedDomainEvent(id, price));
    }
}
