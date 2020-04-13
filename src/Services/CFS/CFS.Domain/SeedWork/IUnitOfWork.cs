using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace CFS.Domain.SeedWork
{
    public interface IUnitOfWork : IDisposable
    {
        void CommitTransaction(IDbTransaction transaction);
    }
}
