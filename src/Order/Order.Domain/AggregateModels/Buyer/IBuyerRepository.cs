namespace Ordering.Domain.AggregateModels.Buyer;

using Ordering.Domain.SeedWork;
using System;
using System.Threading.Tasks;

public interface IBuyerRepository : IRepository<Buyer>
{
    Task AddAsync(Buyer buyer);
    Task<Buyer> GetAsync(Guid buyerId, Guid orderId);
    Task<Card> GetCardAsync(
        int typeId,
        string number,
        string securityNumber,
        string holderName,
        DateTime expiration);

    Task UpdateAsync(Buyer buyer);
}
