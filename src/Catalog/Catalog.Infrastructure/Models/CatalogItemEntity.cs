namespace Catalog.Infrastructure.Models;

using System;

public class CatalogItemEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int CatalogTypeId { get; set; }
    public CatalogTypeEntity CatalogType { get; set; }
    public int CatalogBrandId { get; set; }
    public CatalogBrandEntity CatalogBrand { get; set; }
    public int AvailableStock { get; set; }
}
