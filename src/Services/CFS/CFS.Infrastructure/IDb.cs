using CFS.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFS.Infrastructure
{
    public interface IDb : IUnitOfWork
    {
        Task<T> CommandAsync<T>(Func<IDbConnection, IDbTransaction, Task<T>> command);
        Task<int> ExecuteAsync(string sql, object parameters, Entity entity);
        Task<T> GetAsync<T>(string sql, object parameters);
        Task<IList<T>> SelectAsync<T>(string sql, object parameters);
    }
}
