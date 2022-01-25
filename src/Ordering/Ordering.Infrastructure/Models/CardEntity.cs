namespace Ordering.Infrastructure.Models;

using System;

public class CardEntity
{
    public string Id { get; set; }
    public string BuyerId { get; set; }
    public int Type { get; set; }
    public string Number { get; set; }
    public string SecurityNumber { get; set; }
    public string HolderName { get; set; }
    public DateTime Expiration { get; set; }
}
