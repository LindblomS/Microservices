namespace Management.WebApp.Models;

using System.ComponentModel.DataAnnotations;

public class CreateCatalogItem
{
    [Required, MaxLength(250)]
    public string Name { get; set; }
    [Required, MaxLength(250)]
    public string Description { get; set; }
    [Required, Range(1, double.MaxValue)]
    public decimal Price { get; set; }
    [Required, MaxLength(250)]
    public string Type { get; set; }
    [Required, MaxLength(250)]
    public string Brand { get; set; }
    [Required, Range(1, int.MaxValue)]
    public int AvailableStock { get; set; }
}
