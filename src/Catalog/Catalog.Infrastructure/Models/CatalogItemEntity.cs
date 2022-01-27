namespace Catalog.Infrastructure.Models;

public class CatalogItemEntity
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string CatalogTypeId { get; set; }
    public string CatalogBrandId { get; set; }
    public int AvailableStock { get; set; }
}
