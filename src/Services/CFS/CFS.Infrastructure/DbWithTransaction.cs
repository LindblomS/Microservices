using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFS.Infrastructure
{
    public class DbWithTransaction : IDbWithTransaction
    {
        private readonly IDbTransaction _transaction;
        private readonly ILogger<DbWithTransaction> _logger;

        public DbWithTransaction(IDbTransaction transaction, ILogger<DbWithTransaction> logger)
        {
            _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<T> CommandAsync<T>(Func<IDbConnection, IDbTransaction, Task<T>> command)
        {
            try
            {
                var result = await command(_transaction.Connection, _transaction);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR");
                throw;
            }
        }

        public async Task ExecuteAsync(string sql, object parameters)
        {
            await CommandAsync(async (conn, transaction) =>
            {
                await conn.ExecuteAsync(sql, parameters, transaction);
                return 1;
            });
        }

        public async Task<T> GetAsync<T>(string sql, object parameters)
        {
            return await CommandAsync(async (conn, transaction) =>
            {
                T result = await conn.QuerySingleAsync<T>(sql, parameters, transaction);
                return result;
            });
        }

        public async Task<IList<T>> SelectAsync<T>(string sql, object parameters)
        {
            return await CommandAsync<IList<T>>(async (conn, transaction) =>
            {
                var result = (await conn.QueryAsync<T>(sql, parameters, transaction)).ToList();
                return result;
            });
        }
    }
}
