using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CFS.Infrastructure
{
    public class DbQueries : IDbQueries
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<DbQueries> _logger;

        public DbQueries(IConnectionFactory connectionFactory, ILogger<DbQueries> logger)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public virtual async Task<T> CommandAsync<T>(Func<IDbConnection, Task<T>> command)
        {
            using (var connection = _connectionFactory.GetConnection())
            {
                await connection.OpenAsync();

                try
                {
                    var result = await command(connection);

                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERROR");
                    throw;
                }
            }
        }

        public async Task<T> GetAsync<T>(string sql, object parameters)
        {
            return await CommandAsync(async (conn) =>
            {
                T result = await conn.QuerySingleAsync<T>(sql, parameters);
                return result;
            });
        }

        public async Task<IList<T>> SelectAsync<T>(string sql, object parameters)
        {
            return await CommandAsync<IList<T>>(async (conn) =>
            {
                var result = (await conn.QueryAsync<T>(sql, parameters)).ToList();
                return result;
            });
        }
    }
}

