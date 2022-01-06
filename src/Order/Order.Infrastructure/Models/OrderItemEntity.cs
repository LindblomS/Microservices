namespace Ordering.Infrastructure.Models;

public class OrderItemEntity
{
    public string Id { get; set; }
    public string OrderId { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public int Units { get; set; }
}
