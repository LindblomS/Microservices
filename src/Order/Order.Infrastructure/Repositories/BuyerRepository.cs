namespace Ordering.Infrastructure.Repositories;

using Ordering.Domain.AggregateModels.Buyer;
using Ordering.Infrastructure.EntityFramework;
using Ordering.Infrastructure.Mappers;
using System;
using System.Threading.Tasks;
using System.Linq;

public class BuyerRepository : IBuyerRepository
{
    readonly OrderingContext context;
    const string datetimeFormat = "yyyy-MM-dd";

    public BuyerRepository(OrderingContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task AddAsync(Buyer buyer)
    {
        await context.Buyers.AddAsync(BuyerMapper.Map(buyer));
        await context.SaveChangesAsync();
    }

    public async Task<Buyer> GetAsync(Guid buyerId, Guid orderId)
    {
        return BuyerMapper.Map(await context.Buyers.FindAsync(buyerId), orderId);
    }

    public Task<Card> GetCardAsync(int typeId, string number, string securityNumber, string holderName, DateTime expiration)
    {
        var entity = context.Cards.SingleOrDefault(e => 
            e.Type == typeId &&
            e.Number == number &&
            e.SecurityNumber == securityNumber &&
            e.HolderName == holderName && 
            e.Expiration.ToString(datetimeFormat) == expiration.ToString(datetimeFormat));

        if (entity is null)
            return null;

        return Task.FromResult(BuyerMapper.Map(entity));
    }

    public async Task UpdateAsync(Buyer buyer)
    {
        var updated = BuyerMapper.Map(buyer);
        var original = await context.Buyers.FindAsync(buyer.Id);
        context.Entry(original).CurrentValues.SetValues(updated);
        await context.SaveChangesAsync();
    }
}
