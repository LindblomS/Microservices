namespace Services.Order.Domain.AggregateModels.Buyer;

using Services.Order.Domain.SeedWork;
using System;
using System.Threading.Tasks;

internal interface IBuyerRepository : IRepository<Buyer>
{
    Task<Buyer> CreateAsync(Buyer order);
    Task<Buyer> GetAsync(Guid buyerId);
    Task<Buyer> UpdateAsync(Buyer order);
}
