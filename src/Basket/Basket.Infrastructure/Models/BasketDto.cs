namespace Basket.Infrastructure.Models;

using Basket.Domain.AggregateModels;

internal class BasketDto
{
    public Guid Id { get; set; }
    public IEnumerable<BasketItem> Items { get; set; }
}
