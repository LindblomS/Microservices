namespace Ordering.Infrastructure.Models;

using System;

public class OrderItemEntity
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public string ProductName { get; set; }
    public int UnitPrice { get; set; }
    public int Units { get; set; }
}
