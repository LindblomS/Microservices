using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFS.Infrastructure
{
    public interface IDbQueries : IDb
    {
        Task<T> CommandAsync<T>(Func<IDbConnection, Task<T>> command);
    }
}
