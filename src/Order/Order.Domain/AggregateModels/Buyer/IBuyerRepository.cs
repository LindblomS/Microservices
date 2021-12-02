namespace Ordering.Domain.AggregateModels.Buyer;

using Ordering.Domain.SeedWork;
using System;
using System.Threading.Tasks;

public interface IBuyerRepository : IRepository<Buyer>
{
    Task<Buyer> AddAsync(Buyer order);
    Task<Buyer> GetAsync(Guid buyerId);
}
