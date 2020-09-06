using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace CFS.Domain.SeedWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> CommitTransactionAsync(DbTransaction transaction);
    }
}
