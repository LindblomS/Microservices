using CFS.Domain.Aggregates;
using System;
using System.Threading.Tasks;

namespace CFS.Domain.SeedWork
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
    }
}
