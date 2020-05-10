using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace CFS.Domain.SeedWork
{
    public interface IUnitOfWork : IDisposable
    {
        IDbTransaction BeginTransaction();
        Task<bool> CommitTransaction(IDbTransaction transaction);
        bool HasActiveTransaction();
    }
}
