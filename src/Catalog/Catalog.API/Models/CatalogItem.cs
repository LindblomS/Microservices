namespace Catalog.API.Models;

public class CatalogItem
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public CatalogType Type { get; private set; }
    public CatalogBrand Brand { get; set; }
    public int AvailableStock { get; set; }
}
