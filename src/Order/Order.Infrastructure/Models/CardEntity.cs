namespace Ordering.Infrastructure.Models;

using System;

public class CardEntity
{
    public Guid Id { get; set; }
    public Guid BuyerId { get; set; }
    public int Type { get; set; }
    public string Number { get; set; }
    public string SecurityNumber { get; set; }
    public string HolderName { get; set; }
    public DateTime Expiration { get; set; }
}
