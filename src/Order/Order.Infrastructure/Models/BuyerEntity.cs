namespace Ordering.Infrastructure.Models;

using System.Collections.Generic;

public class BuyerEntity
{
    public string Id { get; set; }
    public string Name { get; set; }
    public ICollection<CardEntity> Cards { get; set; }
}
