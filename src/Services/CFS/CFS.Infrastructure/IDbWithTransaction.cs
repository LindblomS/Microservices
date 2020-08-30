using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFS.Infrastructure
{
    public interface IDbWithTransaction : IDb
    {
        Task<T> CommandAsync<T>(Func<IDbConnection, IDbTransaction, Task<T>> command);
        Task ExecuteAsync(string sql, object parameters);
    }
}
