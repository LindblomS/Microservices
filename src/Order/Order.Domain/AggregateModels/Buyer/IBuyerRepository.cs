namespace Ordering.Domain.AggregateModels.Buyer;

using Ordering.Domain.SeedWork;
using System;
using System.Threading.Tasks;

internal interface IBuyerRepository : IRepository<Buyer>
{
    Task<Buyer> CreateAsync(Buyer order);
    Task<Buyer> GetAsync(Guid buyerId);
    Task<Buyer> UpdateAsync(Buyer order);
}
