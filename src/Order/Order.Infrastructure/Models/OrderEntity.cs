namespace Ordering.Infrastructure.Models;

using System;
using System.Collections.Generic;

public class OrderEntity
{
    public Guid Id { get; set; }
    public ICollection<OrderItemEntity> OrderItems { get; set; }

    public Guid BuyerId { get; set; }
    public BuyerEntity Buyer { get; set; }

    public Guid CardId { get; set; }
    public CardEntity Card { get; set; }

    public int OrderStatusId { get; set; }
    public string Description { get; set; }
    public DateTime Created { get; set; }

    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string ZipCode { get; set; }
}
