namespace Catalog.API.Models;

public class CatalogItem
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public CatalogType Type { get; set; }
    public CatalogBrand Brand { get; set; }
    public int AvailableStock { get; set; }
}
