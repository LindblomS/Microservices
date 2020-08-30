using CFS.Domain.Aggregates;
using System;

namespace CFS.Domain.SeedWork
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository CustomerRepository { get; }
        IFacilityRepository FacilityRepository { get; }
        IServiceRepository ServiceRepository { get; }
        void Commit();
        void BeginTransaction();
    }
}
