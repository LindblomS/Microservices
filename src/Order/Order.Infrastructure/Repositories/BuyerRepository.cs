namespace Ordering.Infrastructure.Repositories;

using Ordering.Domain.AggregateModels.Buyer;
using System;
using System.Threading.Tasks;

public class BuyerRepository : IBuyerRepository
{
    public Task<Buyer> AddAsync(Buyer order)
    {
        throw new NotImplementedException();
    }

    public Task<Buyer> GetAsync(Guid buyerId)
    {
        throw new NotImplementedException();
    }

    public Task<Card> GetCardAsync(int typeId, string number, string securityNumber, string holderName, DateTime expiration)
    {
        throw new NotImplementedException();
    }

    public Task<Buyer> UpdateAsync(Buyer buyer)
    {
        throw new NotImplementedException();
    }
}
